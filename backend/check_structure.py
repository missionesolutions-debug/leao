#!/usr/bin/env python3
"""
Verifica estrutura das tabelas no banco
"""

import sys
import os
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from sqlalchemy import create_engine, text
from app.config.settings import settings

def check_table_structure():
    """Verifica a estrutura das tabelas"""
    try:
        engine = create_engine(settings.DATABASE_URL)
        
        with engine.connect() as connection:
            # Verificar estrutura da tabela users
            print("👥 Estrutura da tabela 'users':")
            result = connection.execute(text("""
                SELECT column_name, data_type, is_nullable
                FROM information_schema.columns 
                WHERE table_name = 'users' 
                ORDER BY ordinal_position
            """))
            
            for row in result.fetchall():
                nullable = "NULL" if row[2] == "YES" else "NOT NULL"
                print(f"   - {row[0]} ({row[1]}) {nullable}")
            
            # Verificar dados dos usuários
            print(f"\n👤 Dados dos usuários:")
            result = connection.execute(text("SELECT * FROM users LIMIT 1"))
            columns = result.keys()
            user_data = result.fetchone()
            
            if user_data:
                for i, col in enumerate(columns):
                    print(f"   - {col}: {user_data[i]}")
            
            # Listar todas as tabelas
            print(f"\n📊 Todas as tabelas no banco:")
            result = connection.execute(text("""
                SELECT table_name 
                FROM information_schema.tables 
                WHERE table_schema = 'public'
                ORDER BY table_name
            """))
            
            for row in result.fetchall():
                print(f"   - {row[0]}")
                
    except Exception as e:
        print(f"❌ Erro: {str(e)}")

if __name__ == "__main__":
    check_table_structure()