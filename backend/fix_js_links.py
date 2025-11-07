#!/usr/bin/env python3
"""
Script para corrigir todos os links marca.html nos arquivos JavaScript
"""

import os
import re

def fix_js_marca_links():
    """Corrige todas as referências marca.html para marca nos arquivos JS"""
    
    frontend_path = os.path.join(os.path.dirname(__file__), "..", "frontend")
    
    # Arquivos JS a serem processados
    js_files = [
        "assets/js/scripts.js",
        "config.js"
    ]
    
    total_fixes = 0
    
    for js_file in js_files:
        file_path = os.path.join(frontend_path, js_file)
        
        if not os.path.exists(file_path):
            print(f"❌ Arquivo não encontrado: {js_file}")
            continue
            
        print(f"🔄 Processando: {js_file}")
        
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
            
            original_content = content
            
            # Padrões de substituição
            patterns = [
                (r'"marca\.html"', '"marca"'),
                (r"'marca\.html'", "'marca'"),
                (r'window\.location\.href\s*=\s*"marca\.html"', 'window.location.href = "marca"'),
                (r'window\.location\.href\s*=\s*\'marca\.html\'', 'window.location.href = "marca"'),
                (r'href\s*=\s*"marca\.html"', 'href = "marca"'),
                (r'href\s*=\s*\'marca\.html\'', 'href = "marca"')
            ]
            
            file_fixes = 0
            for pattern, replacement in patterns:
                matches = len(re.findall(pattern, content))
                if matches > 0:
                    content = re.sub(pattern, replacement, content)
                    file_fixes += matches
            
            if file_fixes > 0:
                with open(file_path, 'w', encoding='utf-8') as f:
                    f.write(content)
                print(f"  ✅ {file_fixes} links corrigidos")
                total_fixes += file_fixes
            else:
                print(f"  ➖ Nenhuma alteração necessária")
                
        except Exception as e:
            print(f"  ❌ Erro ao processar {js_file}: {str(e)}")
    
    print(f"\n🎉 CONCLUÍDO!")
    print(f"📊 Total de links JS corrigidos: {total_fixes}")
    
    # Verificação final
    print(f"\n🔍 Verificação final...")
    remaining_issues = []
    
    for js_file in js_files:
        file_path = os.path.join(frontend_path, js_file)
        if os.path.exists(file_path):
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
            if 'marca.html' in content:
                remaining_issues.append(js_file)
    
    if remaining_issues:
        print(f"⚠️  Ainda há referências 'marca.html' em: {', '.join(remaining_issues)}")
    else:
        print(f"✅ Todos os links JS foram corrigidos!")

if __name__ == "__main__":
    fix_js_marca_links()