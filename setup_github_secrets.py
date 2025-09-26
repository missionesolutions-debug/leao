#!/usr/bin/env python3
"""
Script para configurar secrets do GitHub para deploy no Digital Ocean
"""

import os
import subprocess
import sys

def check_gh_cli():
    """Verifica se GitHub CLI está instalado"""
    try:
        subprocess.run(['gh', '--version'], capture_output=True, check=True)
        return True
    except (subprocess.CalledProcessError, FileNotFoundError):
        return False

def setup_github_secrets():
    """Configura secrets do GitHub para deploy"""
    
    print("🔧 Configurando GitHub Secrets para Digital Ocean Deploy")
    print("=" * 60)
    
    if not check_gh_cli():
        print("❌ GitHub CLI não encontrado!")
        print("📥 Instale o GitHub CLI: https://cli.github.com/")
        print("💡 Ou configure manualmente no GitHub:")
        print("   Repository → Settings → Secrets → Actions")
        return
    
    # Verificar se está logado
    try:
        subprocess.run(['gh', 'auth', 'status'], capture_output=True, check=True)
    except subprocess.CalledProcessError:
        print("🔑 Fazendo login no GitHub...")
        subprocess.run(['gh', 'auth', 'login'])
    
    secrets = {
        'DO_HOST': 'IP do seu servidor Digital Ocean',
        'DO_USERNAME': 'Usuário SSH (geralmente root)',
        'DO_SSH_KEY': 'Conteúdo da sua chave SSH privada',
        'DO_PORT': 'Porta SSH (geralmente 22)'
    }
    
    print("\n📝 Configure os seguintes secrets:")
    print("-" * 40)
    
    for secret_name, description in secrets.items():
        print(f"\n🔐 {secret_name}")
        print(f"   Descrição: {description}")
        
        if secret_name == 'DO_SSH_KEY':
            print("   💡 Dica: Use 'cat ~/.ssh/id_rsa' ou sua chave privada")
            print("   ⚠️  NUNCA compartilhe sua chave privada!")
        
        value = input(f"   Digite o valor para {secret_name}: ").strip()
        
        if value:
            try:
                cmd = ['gh', 'secret', 'set', secret_name, '--body', value]
                subprocess.run(cmd, check=True)
                print(f"   ✅ {secret_name} configurado!")
            except subprocess.CalledProcessError as e:
                print(f"   ❌ Erro ao configurar {secret_name}: {e}")
        else:
            print(f"   ⏭️  Pulando {secret_name}")
    
    print("\n" + "=" * 60)
    print("✅ Configuração de secrets concluída!")
    print("🚀 Agora faça push para main/master para disparar o deploy:")
    print("   git add .")
    print("   git commit -m 'Deploy to Digital Ocean'")
    print("   git push origin main")

def show_manual_setup():
    """Mostra instruções para configuração manual"""
    print("\n📋 CONFIGURAÇÃO MANUAL NO GITHUB:")
    print("=" * 50)
    print("1. Acesse: https://github.com/seu-repo/settings/secrets/actions")
    print("2. Clique em 'New repository secret'")
    print("3. Configure os seguintes secrets:")
    print()
    
    secrets = {
        'DO_HOST': 'IP do servidor Digital Ocean (ex: 143.198.123.45)',
        'DO_USERNAME': 'Usuário SSH (ex: root)',
        'DO_SSH_KEY': 'Conteúdo completo da chave SSH privada',
        'DO_PORT': 'Porta SSH (ex: 22)'
    }
    
    for name, desc in secrets.items():
        print(f"   🔐 {name}: {desc}")
    
    print("\n💡 Para obter sua chave SSH:")
    print("   Linux/Mac: cat ~/.ssh/id_rsa")
    print("   Windows: type %USERPROFILE%\\.ssh\\id_rsa")

if __name__ == "__main__":
    print("🌊 Digital Ocean Deploy Setup")
    print("=" * 40)
    
    choice = input("Usar GitHub CLI para configurar? (y/n): ").lower().strip()
    
    if choice == 'y':
        setup_github_secrets()
    else:
        show_manual_setup()
    
    print("\n📚 Documentação completa: DIGITAL_OCEAN_DEPLOY.md")