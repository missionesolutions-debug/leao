#!/usr/bin/env python3
"""
Script para testar o sistema de login e autenticação
Execute: python test_login.py
"""

import requests
import json

API_URL = "http://localhost:8000"

def test_login():
    """Testar login"""
    print("🔄 Testando login...")
    
    login_data = {
        "email": "jhonatan@gmail.com",
        "password": "1234"
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
    
    # 1. Testar login
    token = test_login()
    if not token:
        return
    
    # 2. Testar /usuarios/me
    user_data = test_me_endpoint(token)
    if not user_data:
        return
    
    # 3. Testar listagem de usuários
    users_list = test_list_users(token)
    
    print("\n✅ Todos os testes concluídos!")

if __name__ == "__main__":
    main()