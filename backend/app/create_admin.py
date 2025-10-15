#!/usr/bin/env python3
"""
Script para criar usuário administrador
Execute: python -m app.create_admin
"""

import sys
import os
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from sqlalchemy.orm import Session
from app.config.database import engine, SessionLocal
from app.models.user_model import User, UserRole
from app.utils.auth import hash_password
from app.config.database import Base

def create_tables():
    """Criar tabelas se não existirem"""
    Base.metadata.create_all(bind=engine)
    print("✅ Tabelas criadas/verificadas")

def create_admin_user():
    """Criar usuário administrador"""
    db: Session = SessionLocal()
    
    try:
        print("🔧 Criando usuário administrador...")
        
        # Dados do admin
        admin_email = input("📧 Email do administrador: ")
        admin_username = input("👤 Nome do administrador: ")
        admin_password = input("🔒 Senha do administrador: ")
        
        # Verificar se já existe
        existing_user = db.query(User).filter(User.email == admin_email).first()
        if existing_user:
            print(f"⚠️ Usuário com email {admin_email} já existe!")
            
            # Opção de promover para admin
            promote = input("Deseja promover este usuário para admin? (s/n): ")
            if promote.lower() == 's':
                existing_user.role = UserRole.ADMIN
                db.commit()
                print(f"✅ Usuário {existing_user.username} promovido para administrador!")
                return
            else:
                print("❌ Operação cancelada")
                return
        
        # Criar novo admin
        hashed_password = hash_password(admin_password)
        
        admin_user = User(
            username=admin_username,
            email=admin_email,
            hashed_password=hashed_password,
            role=UserRole.ADMIN
        )
        
        db.add(admin_user)
        db.commit()
        db.refresh(admin_user)
        
        print(f"✅ Administrador criado com sucesso!")
        print(f"   👤 Nome: {admin_user.username}")
        print(f"   📧 Email: {admin_user.email}")
        print(f"   👑 Role: {admin_user.role}")
        print(f"   🆔 ID: {admin_user.id}")
        
    except Exception as e:
        print(f"❌ Erro ao criar administrador: {e}")
        db.rollback()
    finally:
        db.close()

def list_users():
    """Listar todos os usuários e seus roles"""
    db: Session = SessionLocal()
    
    try:
        users = db.query(User).all()
        
        if not users:
            print("📭 Nenhum usuário encontrado")
            return
            
        print("\n👥 Lista de Usuários:")
        print("-" * 60)
        for user in users:
            role_icon = "👑" if user.role == UserRole.ADMIN else "👤"
            print(f"{role_icon} {user.username} ({user.email}) - {user.role} [ID: {user.id}]")
        print("-" * 60)
        
    except Exception as e:
        print(f"❌ Erro ao listar usuários: {e}")
    finally:
        db.close()

def promote_user_to_admin():
    """Promover usuário existente para administrador"""
    db: Session = SessionLocal()
    
    try:
        # Listar usuários primeiro
        list_users()
        
        user_id = input("\n🆔 Digite o ID do usuário para promover a admin: ")
        
        try:
            user_id = int(user_id)
        except ValueError:
            print("❌ ID deve ser um número")
            return
            
        user = db.query(User).filter(User.id == user_id).first()
        
        if not user:
            print(f"❌ Usuário com ID {user_id} não encontrado")
            return
            
        if user.role == UserRole.ADMIN:
            print(f"⚠️ Usuário {user.username} já é administrador")
            return
            
        user.role = UserRole.ADMIN
        db.commit()
        
        print(f"✅ Usuário {user.username} promovido para administrador!")
        
    except Exception as e:
        print(f"❌ Erro ao promover usuário: {e}")
        db.rollback()
    finally:
        db.close()

def main():
    """Menu principal"""
    create_tables()
    
    while True:
        print("\n" + "="*50)
        print("🔧 GERENCIADOR DE ADMINISTRADORES")
        print("="*50)
        print("1. 👑 Criar novo administrador")
        print("2. 📋 Listar usuários")
        print("3. ⬆️ Promover usuário existente para admin")
        print("4. 🚪 Sair")
        print("="*50)
        
        escolha = input("Escolha uma opção (1-4): ")
        
        if escolha == "1":
            create_admin_user()
        elif escolha == "2":
            list_users()
        elif escolha == "3":
            promote_user_to_admin()
        elif escolha == "4":
            print("👋 Saindo...")
            break
        else:
            print("❌ Opção inválida")

if __name__ == "__main__":
    main()