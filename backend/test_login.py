#!/usr/bin/env python3
"""
Script para testar o sistema de login e verificar senha do admin
Execute: python test_login.py
"""

import requests
import json
import sys
import os
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from sqlalchemy import create_engine, text
from passlib.context import CryptContext
from app.config.settings import settings

API_URL = "http://localhost:8000"

def check_admin_password():
    """Verificar e corrigir senha do admin"""
    print("🔍 Verificando senha do administrador...")
    
    try:
        engine = create_engine(settings.DATABASE_URL)
        pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")
        
        with engine.connect() as connection:
            # Buscar usuário admin
            result = connection.execute(text("""
                SELECT id, username, email, hashed_password, role
                FROM users 
                WHERE email = 'jhonatan@gmail.com'
            """))
            user = result.fetchone()
            
            if not user:
                print("❌ Usuário admin não encontrado!")
                return False
            
            print(f"✅ Admin encontrado: {user[1]} ({user[2]})")
            
            # Testar senhas comuns
            test_passwords = ["1234", "admin", "teste", "password", "admin123"]
            
            for pwd in test_passwords:
                if pwd_context.verify(pwd, user[3]):
                    print(f"✅ Senha atual: '{pwd}'")
                    return pwd
            
            # Se nenhuma senha funcionou, definir uma nova
            print("🔧 Definindo nova senha 'admin123'...")
            new_hash = pwd_context.hash("admin123")
            
            connection.execute(text("""
                UPDATE users 
                SET hashed_password = :hash
                WHERE email = 'jhonatan@gmail.com'
            """), {"hash": new_hash})
            connection.commit()
            
            print("✅ Nova senha definida: 'admin123'")
            return "admin123"
                
    except Exception as e:
        print(f"❌ Erro: {str(e)}")
        return False

def test_login(password="admin123"):
    """Testar login"""
    print(f"🔄 Testando login com senha '{password}'...")
    
    login_data = {
        "email": "jhonatan@gmail.com",
        "password": password
    }
    
    try:
        response = requests.post(f"{API_URL}/auth/login", json=login_data)
        print(f"Status: {response.status_code}")
        
        if response.ok:
            data = response.json()
            print("✅ Login bem-sucedido!")
            print(f"Token: {data['access_token'][:50]}...")
            return data['access_token']
        else:
            print(f"❌ Erro no login: {response.text}")
            return None
    except Exception as e:
        print(f"💥 Erro: {e}")
        return None

def test_me_endpoint(token):
    """Testar endpoint /usuarios/me"""
    print("\n🔄 Testando /usuarios/me...")
    
    headers = {
        "Authorization": f"Bearer {token}",
        "Content-Type": "application/json"
    }
    
    try:
        response = requests.get(f"{API_URL}/usuarios/me", headers=headers)
        print(f"Status: {response.status_code}")
        
        if response.ok:
            data = response.json()
            print("✅ Dados do usuário:")
            print(json.dumps(data, indent=2))
            return data
        else:
            print(f"❌ Erro: {response.text}")
            return None
    except Exception as e:
        print(f"💥 Erro: {e}")
        return None

def test_list_users(token):
    """Testar listagem de usuários"""
    print("\n🔄 Testando /usuarios/...")
    
    headers = {
        "Authorization": f"Bearer {token}",
        "Content-Type": "application/json"
    }
    
    try:
        response = requests.get(f"{API_URL}/usuarios/", headers=headers)
        print(f"Status: {response.status_code}")
        
        if response.ok:
            data = response.json()
            print(f"✅ {len(data)} usuários encontrados:")
            for user in data:
                print(f"  - {user['username']} ({user['email']}) - Role: {user['role']}")
            return data
        else:
            print(f"❌ Erro: {response.text}")
            return None
    except Exception as e:
        print(f"💥 Erro: {e}")
        return None

def main():
    print("🔧 TESTE DE LOGIN E AUTENTICAÇÃO")
    print("=" * 50)
    
    # 0. Verificar/corrigir senha do admin
    admin_password = check_admin_password()
    if not admin_password:
        print("❌ Não foi possível verificar a senha do admin")
        return
    
    print(f"\n🔑 Senha do admin: {admin_password}")
    
    # 1. Testar login
    token = test_login(admin_password)
    if not token:
        return
    
    # 2. Testar /usuarios/me
    user_data = test_me_endpoint(token)
    if not user_data:
        return
    
    # 3. Testar listagem de usuários
    users_list = test_list_users(token)
    
    print("\n✅ Todos os testes concluídos!")
    print(f"💡 Use as credenciais: jhonatan@gmail.com / {admin_password}")

if __name__ == "__main__":
    main()