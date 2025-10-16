#!/usr/bin/env python3
"""
Script para gerenciar usuários administradores
Execute: python create_admin_user.py
"""

import sys
import os
from sqlalchemy.orm import Session
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker

# Adicionar o diretório raiz ao path
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

from app.models.user_model import User, UserRole, Base
from app.utils.auth import hash_password
from app.config.settings import settings

# Criar engine e sessão
engine = create_engine(settings.DATABASE_URL)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

def create_tables():
    """Criar tabelas se não existirem"""
    Base.metadata.create_all(bind=engine)
    print("✅ Tabelas verificadas/criadas")

def create_admin():
    """Criar usuário administrador"""
    print("\n🔧 CRIAR NOVO ADMINISTRADOR")
    print("-" * 40)
    
    username = input("👤 Nome do usuário: ")
    email = input("📧 Email: ")
    password = input("🔒 Senha: ")
    
    db: Session = SessionLocal()
    
    try:
        # Verificar se já existe
        existing = db.query(User).filter(
            (User.email == email) | (User.username == username)
        ).first()
        
        if existing:
            print(f"❌ Usuário já existe: {existing.username} ({existing.email})")
            return
        
        # Criar admin
        hashed_password = hash_password(password)
        admin = User(
            username=username,
            email=email,
            hashed_password=hashed_password,
            role=UserRole.ADMIN
        )
        
        db.add(admin)
        db.commit()
        db.refresh(admin)
        
        print(f"✅ Administrador criado com sucesso!")
        print(f"   ID: {admin.id}")
        print(f"   Nome: {admin.username}")
        print(f"   Email: {admin.email}")
        print(f"   Role: {admin.role}")
        
    except Exception as e:
        print(f"❌ Erro: {e}")
        db.rollback()
    finally:
        db.close()

def list_users():
    """Listar todos os usuários"""
    print("\n👥 LISTA DE USUÁRIOS")
    print("-" * 50)
    
    db: Session = SessionLocal()
    
    try:
        users = db.query(User).all()
        
        if not users:
            print("📭 Nenhum usuário encontrado")
            return
        
        for user in users:
            role_icon = "👑" if user.role == UserRole.ADMIN else "👤"
            print(f"{role_icon} [{user.id:2d}] {user.username:<20} {user.email:<30} ({user.role})")
            
    except Exception as e:
        print(f"❌ Erro: {e}")
    finally:
        db.close()

def promote_user():
    """Promover usuário para admin"""
    list_users()
    
    print("\n⬆️ PROMOVER USUÁRIO PARA ADMIN")
    print("-" * 35)
    
    try:
        user_id = int(input("🆔 ID do usuário: "))
    except ValueError:
        print("❌ ID deve ser um número")
        return
    
    db: Session = SessionLocal()
    
    try:
        user = db.query(User).filter(User.id == user_id).first()
        
        if not user:
            print(f"❌ Usuário com ID {user_id} não encontrado")
            return
        
        if user.role == UserRole.ADMIN:
            print(f"⚠️ {user.username} já é administrador")
            return
        
        user.role = UserRole.ADMIN
        db.commit()
        
        print(f"✅ {user.username} promovido para administrador!")
        
    except Exception as e:
        print(f"❌ Erro: {e}")
        db.rollback()
    finally:
        db.close()

def demote_admin():
    """Rebaixar admin para usuário comum"""
    list_users()
    
    print("\n⬇️ REBAIXAR ADMIN PARA USUÁRIO")
    print("-" * 32)
    
    try:
        user_id = int(input("🆔 ID do administrador: "))
    except ValueError:
        print("❌ ID deve ser um número")
        return
    
    db: Session = SessionLocal()
    
    try:
        user = db.query(User).filter(User.id == user_id).first()
        
        if not user:
            print(f"❌ Usuário com ID {user_id} não encontrado")
            return
        
        if user.role != UserRole.ADMIN:
            print(f"⚠️ {user.username} não é administrador")
            return
        
        # Verificar se não é o último admin
        admin_count = db.query(User).filter(User.role == UserRole.ADMIN).count()
        if admin_count <= 1:
            print("❌ Não é possível remover o último administrador!")
            return
        
        user.role = UserRole.USER
        db.commit()
        
        print(f"✅ {user.username} rebaixado para usuário comum!")
        
    except Exception as e:
        print(f"❌ Erro: {e}")
        db.rollback()
    finally:
        db.close()

def main():
    """Menu principal"""
    create_tables()
    
    while True:
        print("\n" + "="*50)
        print("🔧 GERENCIADOR DE USUÁRIOS - LeãoPy")
        print("="*50)
        print("1. 👑 Criar novo administrador")
        print("2. 👥 Listar usuários")
        print("3. ⬆️ Promover usuário para admin")
        print("4. ⬇️ Rebaixar admin para usuário")
        print("5. 🚪 Sair")
        print("="*50)
        
        escolha = input("Escolha (1-5): ")
        
        if escolha == "1":
            create_admin()
        elif escolha == "2":
            list_users()
        elif escolha == "3":
            promote_user()
        elif escolha == "4":
            demote_admin()
        elif escolha == "5":
            print("👋 Saindo...")
            break
        else:
            print("❌ Opção inválida")

if __name__ == "__main__":
    main()