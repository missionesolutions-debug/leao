#!/usr/bin/env python3
"""
Script para testar a conexão com o novo banco de dados Supabase
"""

import sys
import os
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from sqlalchemy import create_engine, text
from app.config.settings import settings

def test_database_connection():
    """Testa a conexão com o banco de dados"""
    try:
        print("🔍 Testando conexão com o banco de dados...")
        print(f"📍 URL: {settings.DATABASE_URL.split('@')[1]}")  # Não mostra a senha
        
        # Criar engine de teste
        engine = create_engine(settings.DATABASE_URL)
        
        # Testar conexão básica
        with engine.connect() as connection:
            result = connection.execute(text("SELECT version()"))
            version = result.fetchone()[0]
            print(f"✅ Conexão estabelecida com sucesso!")
            print(f"📋 Versão do PostgreSQL: {version}")
            
        # Testar se há tabelas existentes
        with engine.connect() as connection:
            result = connection.execute(text("""
                SELECT table_name 
                FROM information_schema.tables 
                WHERE table_schema = 'public'
                ORDER BY table_name
            """))
            tables = result.fetchall()
            
            if tables:
                print(f"📊 Tabelas encontradas no banco ({len(tables)}):")
                for table in tables[:10]:  # Mostrar apenas as primeiras 10
                    print(f"   - {table[0]}")
                if len(tables) > 10:
                    print(f"   ... e mais {len(tables) - 10} tabelas")
            else:
                print("📋 Banco de dados vazio - nenhuma tabela encontrada")
                
        return True
        
    except Exception as e:
        print(f"❌ Erro ao conectar com o banco de dados:")
        print(f"🔍 Detalhes: {str(e)}")
        return False

def test_database_operations():
    """Testa operações básicas no banco"""
    try:
        print("\n🧪 Testando operações básicas...")
        
        engine = create_engine(settings.DATABASE_URL)
        
        with engine.connect() as connection:
            # Teste de criação de tabela temporária
            connection.execute(text("""
                CREATE TABLE IF NOT EXISTS test_connection (
                    id SERIAL PRIMARY KEY,
                    test_data VARCHAR(100),
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                )
            """))
            connection.commit()
            
            # Teste de inserção
            connection.execute(text("""
                INSERT INTO test_connection (test_data) 
                VALUES ('Teste de conexão - LeaoPy')
            """))
            connection.commit()
            
            # Teste de consulta
            result = connection.execute(text("""
                SELECT id, test_data, created_at 
                FROM test_connection 
                ORDER BY id DESC 
                LIMIT 1
            """))
            row = result.fetchone()
            
            if row:
                print(f"✅ Operações básicas funcionando!")
                print(f"📝 Dados inseridos: ID={row[0]}, Data='{row[1]}'")
            
            # Limpeza - remover tabela de teste
            connection.execute(text("DROP TABLE IF EXISTS test_connection"))
            connection.commit()
            
        return True
        
    except Exception as e:
        print(f"❌ Erro nas operações do banco:")
        print(f"🔍 Detalhes: {str(e)}")
        return False

if __name__ == "__main__":
    print("🚀 Teste de Conexão - Novo Banco Supabase")
    print("=" * 50)
    
    # Teste 1: Conexão básica
    connection_ok = test_database_connection()
    
    if connection_ok:
        # Teste 2: Operações básicas
        operations_ok = test_database_operations()
        
        if operations_ok:
            print("\n🎉 SUCESSO! O banco de dados está pronto para uso.")
            print("💡 Próximos passos:")
            print("   1. Execute as migrações das tabelas")
            print("   2. Crie o usuário administrador")
            print("   3. Reinicie o servidor")
        else:
            print("\n⚠️  Conexão OK, mas há problemas com operações no banco")
    else:
        print("\n❌ Falha na conexão. Verifique as credenciais do banco.")
        
    print("\n" + "=" * 50)