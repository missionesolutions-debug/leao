#!/usr/bin/env python3
"""
Script para corrigir todos os links .html para URLs sem extensão
"""
import os
import re
from pathlib import Path

def fix_html_links():
    """Corrige todos os links .html nos arquivos frontend"""
    
    frontend_path = Path("C:/Users/User 7/Desktop/magnitsky/LeaoPy/frontend")
    
    # Mapeamento das correções (mantendo index.html com ambas as formas)
    replacements = {
        'menu.html': 'menu',
        'marca.html': 'marca', 
        'oraculo.html': 'oraculo',
        'perfil.html': 'perfil',
        'caducidade.html': 'caducidade',
        'nulidade.html': 'nulidade',
        'oposicao.html': 'oposicao',
        'recurso_indeferimento.html': 'recurso_indeferimento',
        'manifestacao_oposicao.html': 'manifestacao_oposicao',
        'manifestacao_recurso.html': 'manifestacao_recurso',
        'contrarazao_nulidade.html': 'contrarazao_nulidade',
        'cadastro.html': 'cadastro'
        # index.html mantém as duas formas (com e sem .html)
    }
    
    # Encontrar todos os arquivos HTML
    html_files = list(frontend_path.glob("*.html"))
    
    total_changes = 0
    
    for html_file in html_files:
        print(f"🔄 Processando: {html_file.name}")
        
        try:
            with open(html_file, 'r', encoding='utf-8') as f:
                content = f.read()
            
            original_content = content
            file_changes = 0
            
            # Aplicar todas as substituições
            for old_url, new_url in replacements.items():
                # Padrões para diferentes tipos de links
                patterns = [
                    (f'href="{old_url}"', f'href="{new_url}"'),
                    (f"href='{old_url}'", f"href='{new_url}'"),
                    (f"window.location.href = '{old_url}'", f"window.location.href = '{new_url}'"),
                    (f'window.location.href = "{old_url}"', f'window.location.href = "{new_url}"'),
                    (f"window.location.href='{old_url}'", f"window.location.href='{new_url}'"),
                    (f'window.location.href="{old_url}"', f'window.location.href="{new_url}"')
                ]
                
                for old_pattern, new_pattern in patterns:
                    if old_pattern in content:
                        content = content.replace(old_pattern, new_pattern)
                        file_changes += content.count(new_pattern) - original_content.count(new_pattern)
            
            # Salvar apenas se houve mudanças
            if content != original_content:
                with open(html_file, 'w', encoding='utf-8') as f:
                    f.write(content)
                print(f"  ✅ {file_changes} links corrigidos")
                total_changes += file_changes
            else:
                print(f"  ➖ Nenhuma alteração necessária")
                
        except Exception as e:
            print(f"  ❌ Erro: {e}")
    
    print(f"\n🎉 CONCLUÍDO!")
    print(f"📊 Total de links corrigidos: {total_changes}")
    print(f"📁 Arquivos processados: {len(html_files)}")
    
    # Verificação final
    print(f"\n🔍 Verificação final...")
    remaining_issues = []
    
    for html_file in html_files:
        try:
            with open(html_file, 'r', encoding='utf-8') as f:
                content = f.read()
            
            # Verificar se ainda há links .html (exceto index.html)
            for old_url in replacements.keys():
                if old_url in content:
                    remaining_issues.append(f"{html_file.name}: {old_url}")
        except:
            pass
    
    if remaining_issues:
        print(f"⚠️  Links ainda pendentes:")
        for issue in remaining_issues[:5]:  # Mostrar apenas os primeiros 5
            print(f"   - {issue}")
        if len(remaining_issues) > 5:
            print(f"   ... e mais {len(remaining_issues) - 5}")
    else:
        print(f"✅ Todos os links foram corrigidos!")

if __name__ == "__main__":
    print("🛠️  CORREÇÃO AUTOMÁTICA DE LINKS HTML")
    print("=" * 50)
    fix_html_links()