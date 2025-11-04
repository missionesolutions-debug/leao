#!/usr/bin/env python3
"""
Teste para verificar se o bcrypt está funcionando
"""

import sys
import os
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

try:
    from passlib.context import CryptContext
    
    print("🔍 Testando bcrypt...")
    
    # Criar contexto de senha como usado na aplicação
    pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")
    
    # Testar hash de senha
    test_password = "teste123"
    hashed = pwd_context.hash(test_password)
    print(f"✅ Hash criado com sucesso: {hashed[:50]}...")
    
    # Testar verificação de senha
    is_valid = pwd_context.verify(test_password, hashed)
    print(f"✅ Verificação de senha: {'OK' if is_valid else 'FALHOU'}")
    
    print("🎉 bcrypt funcionando corretamente!")
    
except Exception as e:
    print(f"❌ Erro no bcrypt: {str(e)}")
    print("Tentando soluções alternativas...")
    
    # Tentar importar bcrypt diretamente
    try:
        import bcrypt
        print(f"✅ bcrypt versão: {bcrypt.__version__}")
        
        # Teste direto
        password = b"teste123"
        hashed = bcrypt.hashpw(password, bcrypt.gensalt())
        print("✅ bcrypt direto funcionando!")
        
    except Exception as e2:
        print(f"❌ Erro bcrypt direto: {str(e2)}")