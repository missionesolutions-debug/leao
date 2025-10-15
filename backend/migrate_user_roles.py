#!/usr/bin/env python3
"""
Migração para adicionar campo 'role' aos usuários existentes
Execute: python migrate_user_roles.py
"""

import sys
import os
from sqlalchemy.orm import Session
from sqlalchemy import create_engine, text
from sqlalchemy.orm import sessionmaker

# Adicionar o diretório raiz ao path
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

from app.models.user_model import User, UserRole, Base
from app.config.settings import settings

# Criar engine e sessão
engine = create_engine(settings.DATABASE_URL)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

def check_column_exists():
    """Verificar se a coluna 'role' já existe"""
    db: Session = SessionLocal()
    
    try:
        # Tentar executar uma query que usa a coluna role
        result = db.execute(text("SELECT role FROM users LIMIT 1")).fetchone()
        return True
    except Exception:
        return False
    finally:
        db.close()

def add_role_column():
    """Adicionar coluna 'role' se não existir"""
    if check_column_exists():
        print("✅ Coluna 'role' já existe")
        return True
    
    print("🔧 Adicionando coluna 'role' à tabela users...")
    
    db: Session = SessionLocal()
    
    try:
        # Adicionar coluna role
        db.execute(text("ALTER TABLE users ADD COLUMN role VARCHAR(10) DEFAULT 'USER'"))
        
        # Atualizar todos os usuários existentes para USER
        db.execute(text("UPDATE users SET role = 'USER' WHERE role IS NULL"))
        
        db.commit()
        print("✅ Coluna 'role' adicionada com sucesso!")
        return True
        
    except Exception as e:
        print(f"❌ Erro ao adicionar coluna: {e}")
        db.rollback()
        return False
    finally:
        db.close()

def migrate_existing_users():
    """Migrar usuários existentes"""
    print("\n🔄 Iniciando migração...")
    
    # Criar tabelas
    Base.metadata.create_all(bind=engine)
    
    # Adicionar coluna role se necessário
    if not add_role_column():
        return False
    
    db: Session = SessionLocal()
    
    try:
        # Verificar usuários
        users = db.query(User).all()
        
        print(f"📊 Encontrados {len(users)} usuários")
        
        # Garantir que todos tenham role definida
        updated_count = 0
        for user in users:
            if not user.role:
                user.role = UserRole.USER
                updated_count += 1
        
        if updated_count > 0:
            db.commit()
            print(f"✅ {updated_count} usuários atualizados com role 'USER'")
        else:
            print("✅ Todos os usuários já possuem role definida")
        
        return True
        
    except Exception as e:
        print(f"❌ Erro na migração: {e}")
        db.rollback()
        return False
    finally:
        db.close()

def create_first_admin():
    """Criar primeiro administrador se não houver nenhum"""
    db: Session = SessionLocal()
    
    try:
        # Verificar se já existe admin
        admin_count = db.query(User).filter(User.role == UserRole.ADMIN).count()
        
        if admin_count > 0:
            print(f"✅ Já existem {admin_count} administrador(es)")
            return True
        
        print("\n👑 CRIAR PRIMEIRO ADMINISTRADOR")
        print("-" * 35)
        print("⚠️ Nenhum administrador encontrado!")
        
        create = input("Deseja criar o primeiro admin agora? (s/N): ").lower()
        
        if create != 's':
            print("⚠️ Sistema sem administrador!")
            return False
        
        # Listar usuários existentes
        users = db.query(User).filter(User.role == UserRole.USER).all()
        
        if users:
            print("\n👥 Usuários disponíveis para promover:")
            for i, user in enumerate(users, 1):
                print(f"{i}. {user.username} ({user.email})")
            
            print("0. Criar novo usuário admin")
            
            try:
                escolha = int(input("\nEscolha (0 para novo): "))
                
                if escolha == 0:
                    # Criar novo admin
                    from app.utils.auth import hash_password
                    
                    username = input("👤 Nome do usuário: ")
                    email = input("📧 Email: ")
                    password = input("🔒 Senha: ")
                    
                    admin = User(
                        username=username,
                        email=email,
                        hashed_password=hash_password(password),
                        role=UserRole.ADMIN
                    )
                    
                    db.add(admin)
                    db.commit()
                    
                    print(f"✅ Administrador '{username}' criado com sucesso!")
                    
                elif 1 <= escolha <= len(users):
                    # Promover usuário existente
                    user = users[escolha - 1]
                    user.role = UserRole.ADMIN
                    db.commit()
                    
                    print(f"✅ {user.username} promovido para administrador!")
                    
                else:
                    print("❌ Opção inválida")
                    return False
                
            except ValueError:
                print("❌ Entrada inválida")
                return False
        else:
            # Criar primeiro admin
            from app.utils.auth import hash_password
            
            print("\n👤 Criando primeiro administrador:")
            username = input("Nome do usuário: ")
            email = input("Email: ")
            password = input("Senha: ")
            
            admin = User(
                username=username,
                email=email,
                hashed_password=hash_password(password),
                role=UserRole.ADMIN
            )
            
            db.add(admin)
            db.commit()
            
            print(f"✅ Primeiro administrador '{username}' criado!")
        
        return True
        
    except Exception as e:
        print(f"❌ Erro: {e}")
        db.rollback()
        return False
    finally:
        db.close()

def main():
    """Executar migração completa"""
    print("🔧 MIGRAÇÃO DE USUÁRIOS - LeãoPy")
    print("="*40)
    
    # 1. Migrar estrutura
    if not migrate_existing_users():
        print("❌ Falha na migração")
        return
    
    # 2. Criar primeiro admin se necessário
    create_first_admin()
    
    print("\n✅ Migração concluída!")
    print("\n📋 Próximos passos:")
    print("1. Execute 'python create_admin_user.py' para gerenciar usuários")
    print("2. Acesse o sistema com as credenciais do administrador")

if __name__ == "__main__":
    main()