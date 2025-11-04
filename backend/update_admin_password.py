#!/usr/bin/env python3
"""
Script simples para definir senha do admin
"""

import sys
import os
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from sqlalchemy import create_engine, text
from passlib.context import CryptContext
from app.config.settings import settings

def update_admin_password():
    """Atualizar senha do admin para 'admin123'"""
    try:
        print("🔧 Atualizando senha do admin...")
        
        engine = create_engine(settings.DATABASE_URL)
        pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")
        
        # Hash da nova senha
        new_hash = pwd_context.hash("admin123")
        
        with engine.connect() as connection:
            result = connection.execute(text("""
                UPDATE users 
                SET hashed_password = :hash 
                WHERE email = 'jhonatan@gmail.com'
            """), {"hash": new_hash})
            connection.commit()
            
            if result.rowcount > 0:
                print("✅ Senha atualizada com sucesso!")
                print("📧 Email: jhonatan@gmail.com")
                print("🔑 Senha: admin123")
            else:
                print("❌ Usuário não encontrado")
                
    except Exception as e:
        print(f"❌ Erro: {str(e)}")

if __name__ == "__main__":
    update_admin_password()