#!/usr/bin/env python3
"""
Script de teste para a API de petições
"""

import requests
import json

def test_peticao_caducidade():
    """Testa a criação de uma petição de caducidade"""
    
    # Dados de teste
    test_data = {
        "registro_requerida": "123456789",
        "marca_requerida": "Marca Teste",
        "classe_requerida": "35",
        "especificacoes_requerida": "Serviços de marketing e consultoria",
        "titular_requerida": "Empresa Teste Ltda",
        "processo_requerida": "987654321",
        "nome_cliente": "Cliente Teste",
        "marca_cliente": "Marca Cliente",
        "data_deposito_cliente": "15/03/2023",
        "classe_cliente": "35"
    }
    
    # URL da API
    url = "http://127.0.0.1:8000/peticoes/caducidade"
    
    print(f"🧪 Testando API: {url}")
    print(f"📦 Dados: {json.dumps(test_data, indent=2)}")
    
    try:
        # Fazer requisição POST
        response = requests.post(url, json=test_data, timeout=30)
        
        print(f"\n📊 Status: {response.status_code}")
        print(f"📄 Response: {response.text[:500]}...")
        
        if response.status_code == 200:
            print("✅ Teste PASSOU - Petição criada com sucesso!")
            return True
        else:
            print(f"❌ Teste FALHOU - Status {response.status_code}")
            return False
            
    except requests.exceptions.RequestException as e:
        print(f"❌ Erro de conexão: {e}")
        return False
    except Exception as e:
        print(f"❌ Erro inesperado: {e}")
        return False

def test_listar_peticoes():
    """Testa a listagem de petições"""
    
    url = "http://127.0.0.1:8000/peticoes/"
    
    print(f"\n🧪 Testando listagem: {url}")
    
    try:
        response = requests.get(url, timeout=10)
        
        print(f"📊 Status: {response.status_code}")
        print(f"📄 Response: {response.text[:300]}...")
        
        if response.status_code == 200:
            print("✅ Listagem funcionando!")
            return True
        else:
            print(f"❌ Erro na listagem - Status {response.status_code}")
            return False
            
    except Exception as e:
        print(f"❌ Erro: {e}")
        return False

if __name__ == "__main__":
    print("🔬 TESTE DA API DE PETIÇÕES")
    print("=" * 40)
    
    # Testar listagem primeiro
    test1 = test_listar_peticoes()
    
    # Testar criação
    test2 = test_peticao_caducidade()
    
    print("\n" + "=" * 40)
    if test1 and test2:
        print("🎉 TODOS OS TESTES PASSARAM!")
    else:
        print("❌ ALGUNS TESTES FALHARAM")