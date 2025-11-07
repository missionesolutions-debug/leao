#!/usr/bin/env python3
"""
Script de teste para validar a estrutura otimizada das rotas
"""
import requests
import json
from datetime import datetime

API_BASE = "http://localhost:8000"

def test_health():
    """Teste do endpoint de health"""
    try:
        response = requests.get(f"{API_BASE}/health")
        if response.status_code == 200:
            data = response.json()
            print(f"✅ Health: {data['status']} - v{data['version']}")
            return True
    except Exception as e:
        print(f"❌ Health failed: {e}")
    return False

def test_frontend_routes():
    """Teste das rotas frontend"""
    routes = [
        "/", "/index.html", "/menu.html", "/marca.html", 
        "/oraculo.html", "/perfil.html", "/caducidade.html",
        "/config.js"
    ]
    
    success = 0
    for route in routes:
        try:
            response = requests.get(f"{API_BASE}{route}")
            if response.status_code == 200:
                print(f"✅ Frontend: {route}")
                success += 1
            else:
                print(f"❌ Frontend: {route} - {response.status_code}")
        except Exception as e:
            print(f"❌ Frontend: {route} - {e}")
    
    return success, len(routes)

def test_api_structure():
    """Teste da estrutura da API versioned"""
    endpoints = [
        "/api/v1/auth/register",  # Deve retornar 422 (sem dados)
        "/api/v1/usuarios/count", # Deve funcionar (público)
    ]
    
    success = 0
    for endpoint in endpoints:
        try:
            if "register" in endpoint:
                # POST sem dados deve dar 422
                response = requests.post(f"{API_BASE}{endpoint}")
                expected = 422
            else:
                # GET público
                response = requests.get(f"{API_BASE}{endpoint}")
                expected = 200
                
            if response.status_code == expected:
                print(f"✅ API: {endpoint} - {response.status_code}")
                success += 1
            else:
                print(f"⚠️  API: {endpoint} - {response.status_code} (esperado {expected})")
        except Exception as e:
            print(f"❌ API: {endpoint} - {e}")
    
    return success, len(endpoints)

def test_docs():
    """Teste da documentação automática"""
    try:
        response = requests.get(f"{API_BASE}/docs")
        if response.status_code == 200:
            print("✅ Swagger Docs: Acessível")
            return True
    except Exception as e:
        print(f"❌ Swagger Docs: {e}")
    return False

def main():
    print("🧪 TESTE DA ESTRUTURA OTIMIZADA")
    print("=" * 50)
    print(f"📅 Data: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    print(f"🌐 Base URL: {API_BASE}")
    print()
    
    # Teste 1: Health Check
    print("1️⃣ Testando Health Check...")
    health_ok = test_health()
    print()
    
    # Teste 2: Rotas Frontend 
    print("2️⃣ Testando Rotas Frontend...")
    frontend_success, frontend_total = test_frontend_routes()
    print()
    
    # Teste 3: Estrutura API
    print("3️⃣ Testando Estrutura API v1...")
    api_success, api_total = test_api_structure()
    print()
    
    # Teste 4: Documentação
    print("4️⃣ Testando Documentação...")
    docs_ok = test_docs()
    print()
    
    # Resumo
    print("📊 RESUMO DOS TESTES")
    print("=" * 50)
    print(f"✅ Health Check: {'OK' if health_ok else 'FALHOU'}")
    print(f"✅ Frontend: {frontend_success}/{frontend_total} rotas OK")
    print(f"✅ API v1: {api_success}/{api_total} endpoints OK")
    print(f"✅ Documentação: {'OK' if docs_ok else 'FALHOU'}")
    
    total_tests = 2 + frontend_total + api_total
    passed_tests = (1 if health_ok else 0) + frontend_success + api_success + (1 if docs_ok else 0)
    
    print(f"\n🎯 RESULTADO FINAL: {passed_tests}/{total_tests} ({(passed_tests/total_tests)*100:.1f}%)")
    
    if passed_tests == total_tests:
        print("🎉 TODOS OS TESTES PASSARAM!")
        print("✨ Sistema otimizado funcionando perfeitamente!")
    else:
        print("⚠️  Alguns testes falharam - verificar logs acima")
    
    print("\n💡 Próximos passos:")
    print("   - Acesse http://localhost:8000/docs para ver todas as rotas")
    print("   - Teste login em http://localhost:8000/index.html")
    print("   - Estrutura otimizada pronta para manutenção!")

if __name__ == "__main__":
    main()