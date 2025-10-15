#!/usr/bin/env python3
"""
Teste para verificar o controle de acesso por role
"""

import requests
import json

# Configurações
BASE_URL = "http://localhost:8000"

def test_user_access():
    print("🔧 TESTE DE CONTROLE DE ACESSO POR ROLE")
    print("=" * 50)
    
    # Teste 1: Login com usuário comum
    print("🔄 Testando login com usuário comum...")
    
    login_data = {
        "email": "jhonatan.bandidor@gmail.com",  # Usuário com role 'user'
        "password": "1234"  # Assumindo que todos têm a mesma senha
    }
    
    try:
        response = requests.post(f"{BASE_URL}/auth/login", json=login_data)
        print(f"Status: {response.status_code}")
        
        if response.status_code == 200:
            data = response.json()
            token = data["access_token"]
            print("✅ Login bem-sucedido!")
            print(f"Token: {token[:50]}...")
        else:
            print("❌ Falha no login:", response.text)
            return
            
    except Exception as e:
        print(f"❌ Erro na requisição de login: {e}")
        return
    
    # Headers para próximas requisições
    headers = {
        "Authorization": f"Bearer {token}",
        "Content-Type": "application/json"
    }
    
    # Teste 2: Verificar dados do usuário atual
    print("\n🔄 Testando /usuarios/me...")
    try:
        response = requests.get(f"{BASE_URL}/usuarios/me", headers=headers)
        print(f"Status: {response.status_code}")
        
        if response.status_code == 200:
            user_data = response.json()
            print("✅ Dados do usuário:")
            print(json.dumps(user_data, indent=2, ensure_ascii=False))
            
            user_role = user_data.get("role")
            print(f"\n👤 Role do usuário: {user_role}")
            print(f"🔍 É admin? {user_role == 'admin'}")
            
        else:
            print("❌ Erro ao obter dados do usuário:", response.text)
            return
            
    except Exception as e:
        print(f"❌ Erro na requisição /usuarios/me: {e}")
        return
    
    # Teste 3: Verificar lista de usuários
    print("\n🔄 Testando /usuarios/...")
    try:
        response = requests.get(f"{BASE_URL}/usuarios/", headers=headers)
        print(f"Status: {response.status_code}")
        
        if response.status_code == 200:
            usuarios = response.json()
            print(f"✅ {len(usuarios)} usuário(s) retornado(s):")
            
            for user in usuarios:
                print(f"  - {user['username']} ({user['email']}) - Role: {user['role']} - ID: {user['id']}")
                
            # Análise do resultado
            print(f"\n📊 ANÁLISE:")
            print(f"Total de usuários retornados: {len(usuarios)}")
            
            if len(usuarios) == 1:
                print("✅ Correto! Usuário comum vê apenas seus próprios dados")
                returned_user = usuarios[0]
                if returned_user['email'] == login_data['email']:
                    print("✅ O usuário retornado é o mesmo que fez login")
                else:
                    print("❌ ERRO: Usuário retornado não é o que fez login!")
            else:
                print("❌ ERRO: Usuário comum está vendo dados de outros usuários!")
                
        else:
            print("❌ Erro ao listar usuários:", response.text)
            
    except Exception as e:
        print(f"❌ Erro na requisição /usuarios/: {e}")
    
    print("\n✅ Teste concluído!")

if __name__ == "__main__":
    test_user_access()