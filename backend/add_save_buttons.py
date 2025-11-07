#!/usr/bin/env python3
"""
Script para adicionar botões de salvamento em todas as páginas de petições
"""

import os
import re

def add_save_buttons_to_petition_pages():
    """Adiciona botões de salvamento a todas as páginas de petições"""
    
    frontend_path = os.path.join(os.path.dirname(__file__), "..", "frontend")
    
    # Lista de arquivos de petições para atualizar
    petition_files = [
        "nulidade.html",
        "oposicao.html", 
        "recurso_indeferimento.html",
        "manifestacao_oposicao.html",
        "manifestacao_recurso.html",
        "contrarazao_nulidade.html"
    ]
    
    # Botões HTML para adicionar
    save_buttons_html = '''          <button type="button" class="btn btn-success me-2" onclick="salvarPeticao()">
            <em class="icon ni ni-save"></em> Salvar Petição
          </button>
          <button type="button" class="btn btn-primary me-2" onclick="baixarPeticao()">
            <em class="icon ni ni-download"></em> Baixar PDF
          </button>'''
    
    # Funções JavaScript para adicionar
    save_functions_js = '''
    // ===== FUNÇÕES DE SALVAMENTO =====
    
    function salvarPeticao() {
      if (!dadosItemAtual || !dadosItemAtual.id) {
        alert('Nenhuma petição para salvar.');
        return;
      }
      
      const botao = event.target;
      const textoOriginal = botao.innerHTML;
      botao.innerHTML = '<em class="icon ni ni-loader"></em> Salvando...';
      botao.disabled = true;
      
      const dadosSalvamento = {
        id: dadosItemAtual.id,
        tipo: tipoItemAtual,
        titulo: `Petição ${tipoItemAtual} - ${new Date().toLocaleDateString('pt-BR')}`,
        conteudo: document.getElementById('conteudoPeticaoTexto') ? document.getElementById('conteudoPeticaoTexto').textContent : 'Conteúdo não encontrado',
        data_salvamento: new Date().toISOString()
      };
      
      fetch(`${API_URL}/peticoes/${dadosItemAtual.id}/salvar`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('token')}`
        },
        body: JSON.stringify(dadosSalvamento)
      })
      .then(response => response.json())
      .then(data => {
        botao.innerHTML = textoOriginal;
        botao.disabled = false;
        
        if (data.success) {
          alert('✅ Petição salva com sucesso!');
        } else {
          alert('Erro ao salvar petição: ' + (data.message || 'Erro desconhecido'));
        }
      })
      .catch(error => {
        botao.innerHTML = textoOriginal;
        botao.disabled = false;
        console.error('Erro ao salvar:', error);
        alert('Erro ao salvar petição. Tente novamente.');
      });
    }
    
    function baixarPeticao() {
      if (!dadosItemAtual || !dadosItemAtual.id) {
        alert('Nenhuma petição para baixar.');
        return;
      }
      
      const botao = event.target;
      const textoOriginal = botao.innerHTML;
      botao.innerHTML = '<em class="icon ni ni-loader"></em> Gerando PDF...';
      botao.disabled = true;
      
      fetch(`${API_URL}/peticoes/${dadosItemAtual.id}/pdf`, {
        method: 'GET',
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      })
      .then(response => {
        if (!response.ok) throw new Error('Erro ao gerar PDF');
        return response.blob();
      })
      .then(blob => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = url;
        a.download = `peticao_${tipoItemAtual}_${new Date().toLocaleDateString('pt-BR').replace(/\//g, '-')}.pdf`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        
        botao.innerHTML = textoOriginal;
        botao.disabled = false;
        alert('✅ PDF baixado com sucesso!');
      })
      .catch(error => {
        botao.innerHTML = textoOriginal;
        botao.disabled = false;
        console.error('Erro ao baixar PDF:', error);
        alert('Erro ao gerar PDF. Tente novamente.');
      });
    }'''
    
    total_updated = 0
    
    for petition_file in petition_files:
        file_path = os.path.join(frontend_path, petition_file)
        
        if not os.path.exists(file_path):
            print(f"⚠️  Arquivo não encontrado: {petition_file}")
            continue
            
        print(f"🔄 Processando: {petition_file}")
        
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
            
            # Verificar se já tem a estrutura de modal
            if 'modalEdicaoAvancada' in content:
                print(f"  ✅ {petition_file} já possui estrutura de modal")
                
                # Verificar se já tem os botões de salvamento
                if 'salvarPeticao()' in content:
                    print(f"  ℹ️  {petition_file} já possui botões de salvamento")
                else:
                    # Adicionar apenas os botões se não existirem
                    # Procurar por onde inserir os botões
                    button_pattern = r'(<button[^>]*onclick="reenviarComObservacoes\(\)"[^>]*>.*?</button>\s*<button[^>]*onclick="fecharModalEdicaoAvancada\(\)"[^>]*>.*?</button>)'
                    match = re.search(button_pattern, content, re.DOTALL)
                    
                    if match:
                        # Inserir os botões de salvamento antes dos botões existentes
                        new_buttons = f'''{save_buttons_html}
          <button type="button" class="btn btn-info me-2" onclick="reenviarComObservacoes()">
            <em class="icon ni ni-repeat"></em> Reenviar com Observações
          </button>
          <button type="button" class="btn btn-secondary" onclick="fecharModalEdicaoAvancada()">Fechar</button>'''
                        
                        content = content.replace(match.group(1), new_buttons)
                    
                    # Adicionar as funções JavaScript antes do fechamento do script
                    script_end_pattern = r'(\s*)(</script>)(\s*</body>)'
                    content = re.sub(script_end_pattern, f'\\1{save_functions_js}\\1\\2\\3', content)
                    
                    with open(file_path, 'w', encoding='utf-8') as f:
                        f.write(content)
                    
                    print(f"  ✅ Botões adicionados a {petition_file}")
                    total_updated += 1
            else:
                print(f"  ⚠️  {petition_file} não possui estrutura de modal - pulando")
                
        except Exception as e:
            print(f"  ❌ Erro ao processar {petition_file}: {str(e)}")
    
    print(f"\n🎉 CONCLUÍDO!")
    print(f"📊 Total de arquivos atualizados: {total_updated}")
    print(f"📋 Botões adicionados: Salvar Petição, Baixar PDF")

if __name__ == "__main__":
    print("🔧 ADICIONANDO BOTÕES DE SALVAMENTO")
    print("=" * 45)
    add_save_buttons_to_petition_pages()