#!/usr/bin/env python3
"""
Teste avançado de conexão com Supabase
"""

import sys
import os
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

import psycopg2
from sqlalchemy import create_engine
import urllib.parse

def test_direct_psycopg2():
    """Testa conexão direta com psycopg2"""
    try:
        print("🔍 Testando conexão direta com psycopg2...")
        
        # Credenciais do novo banco
        connection = psycopg2.connect(
            host="db.sijropxehjxbgyhatagn.supabase.co",
            port=5432,
            database="postgres", 
            user="postgres",
            password="_9&5G9sief@pWdd",
            connect_timeout=10
        )
        
        cursor = connection.cursor()
        cursor.execute("SELECT version()")
        version = cursor.fetchone()[0]
        
        print(f"✅ Conexão psycopg2 estabelecida!")
        print(f"📋 Versão: {version}")
        
        cursor.close()
        connection.close()
        return True
        
    except Exception as e:
        print(f"❌ Erro psycopg2: {str(e)}")
        return False

def test_sqlalchemy_urls():
    """Testa diferentes formatos de URL com SQLAlchemy"""
    
    # Diferentes formatos de URL para testar
    urls = [
        # URL original (com encoding)
        "postgresql://postgres:_9%265G9sief%40pWdd@db.sijropxehjxbgyhatagn.supabase.co:5432/postgres",
        
        # URL com psycopg2 explícito
        "postgresql+psycopg2://postgres:_9%265G9sief%40pWdd@db.sijropxehjxbgyhatagn.supabase.co:5432/postgres",
        
        # URL com encoding diferente
        f"postgresql://postgres:{urllib.parse.quote_plus('_9&5G9sief@pWdd')}@db.sijropxehjxbgyhatagn.supabase.co:5432/postgres",
    ]
    
    for i, url in enumerate(urls, 1):
        try:
            print(f"\n🧪 Teste {i}: Testando URL format {i}...")
            print(f"📍 URL: {url.split('@')[0].split('//')[1].split(':')[0]}@{url.split('@')[1]}")
            
            engine = create_engine(url, connect_args={"connect_timeout": 10})
            
            with engine.connect() as connection:
                result = connection.execute("SELECT 1 as test")
                test_result = result.fetchone()[0]
                
            if test_result == 1:
                print(f"✅ Formato {i} funcionou!")
                return url
                
        except Exception as e:
            print(f"❌ Formato {i} falhou: {str(e)}")
            continue
    
    return None

def main():
    print("🚀 Teste Avançado - Conexão Supabase")
    print("=" * 50)
    
    # Teste 1: Conexão direta
    direct_ok = test_direct_psycopg2()
    
    if direct_ok:
        print("\n✅ Conexão direta funciona! Testando URLs SQLAlchemy...")
        working_url = test_sqlalchemy_urls()
        
        if working_url:
            print(f"\n🎉 SUCESSO! URL funcionando encontrada:")
            print(f"📝 Use esta URL no .env:")
            print(f"DATABASE_URL={working_url}")
            
            # Atualizar o arquivo .env automaticamente
            env_path = "app/.env"
            if os.path.exists(env_path):
                with open(env_path, 'r') as f:
                    content = f.read()
                
                # Substituir a linha DATABASE_URL
                lines = content.split('\n')
                for i, line in enumerate(lines):
                    if line.startswith('DATABASE_URL='):
                        lines[i] = f"DATABASE_URL={working_url}"
                        break
                
                with open(env_path, 'w') as f:
                    f.write('\n'.join(lines))
                
                print(f"✅ Arquivo .env atualizado automaticamente!")
            
        else:
            print("\n⚠️  Conexão direta OK, mas SQLAlchemy com problema")
    else:
        print("\n❌ Problema na conexão básica. Verificar:")
        print("   1. Credenciais corretas?")
        print("   2. Firewall/proxy bloqueando?") 
        print("   3. Supabase ativo?")
    
    print("\n" + "=" * 50)

if __name__ == "__main__":
    main()