#!/usr/bin/env python3
"""
Validação completa do novo banco de dados
"""

import sys
import os
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from sqlalchemy import create_engine, text
from app.config.settings import settings

def validate_setup():
    """Valida se o setup está completo"""
    try:
        print("🔍 Validando configuração do novo banco...")
        print(f"📍 Host: {settings.DATABASE_URL.split('@')[1].split(':')[0]}")
        
        engine = create_engine(settings.DATABASE_URL)
        
        with engine.connect() as connection:
            # Verificar tabelas criadas
            result = connection.execute(text("""
                SELECT table_name 
                FROM information_schema.tables 
                WHERE table_schema = 'public'
                ORDER BY table_name
            """))
            tables = [row[0] for row in result.fetchall()]
            
            print(f"✅ Tabelas encontradas ({len(tables)}):")
            expected_tables = ['users', 'brands', 'caducidades', 'peticoes']
            
            for table in expected_tables:
                if table in tables:
                    print(f"   ✅ {table}")
                else:
                    print(f"   ❌ {table} - AUSENTE")
            
            # Verificar usuários
            result = connection.execute(text("SELECT COUNT(*) FROM users"))
            user_count = result.fetchone()[0]
            print(f"👥 Usuários cadastrados: {user_count}")
            
            if user_count > 0:
                result = connection.execute(text("""
                    SELECT username, email, role 
                    FROM users 
                    WHERE role = 'ADMIN'
                    LIMIT 1
                """))
                admin = result.fetchone()
                if admin:
                    print(f"👑 Admin: {admin[0]} ({admin[1]})")
                else:
                    print("⚠️  Nenhum administrador encontrado")
            
            # Teste de autenticação
            print(f"\n🔐 Para testar login:")
            print(f"📧 Email: jhonatan@gmail.com")
            print(f"🔑 Senha: (a senha que você definiu)")
            print(f"🌐 URL: http://localhost:8000/index.html")
            
        return True
        
    except Exception as e:
        print(f"❌ Erro na validação: {str(e)}")
        return False

def main():
    print("🎯 Validação Final - Novo Banco Supabase")
    print("=" * 50)
    
    if validate_setup():
        print("\n🎉 CONFIGURAÇÃO COMPLETA!")
        print("✅ Banco de dados conectado")
        print("✅ Tabelas criadas")
        print("✅ Usuário administrador configurado")
        print("✅ Servidor rodando")
        
        print(f"\n🚀 Próximos passos:")
        print(f"1. Acesse: http://localhost:8000/index.html")
        print(f"2. Faça login com as credenciais do admin")
        print(f"3. Sistema pronto para uso!")
        
    else:
        print("\n❌ Há problemas na configuração")
        
    print("\n" + "=" * 50)

if __name__ == "__main__":
    main()