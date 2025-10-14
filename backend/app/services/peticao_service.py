from app.integrations.openai_client import OpenAIClient
from app.repositories.peticao_repository import PeticaoRepository
from datetime import datetime

class PeticaoService:
    def __init__(self, db, openai_client: OpenAIClient):
        self.db = db
        self.openai_client = openai_client
        self.repository = PeticaoRepository(db)

    def gerar_caducidade(self, dados):
        data_atual = datetime.now().strftime("%d/%m/%Y")
        prompt = f"""
Elabore uma PETIÇÃO DE CADUCIDADE COMPLETA para o INPI,
seguindo exatamente o modelo abaixo,
apenas substituindo os dados conforme as informações fornecidas.
Mantenha a estrutura, a linguagem formal e os parágrafos conforme o exemplo.

MODELO DE REFERÊNCIA:
---
ILMO. SR. EXAMINADOR DA DIRETORIA DE MARCAS DO INSTITUTO NACIONAL DA PROPRIEDADE INDUSTRIAL – INPI

REGISTRO Nº: 91023123
MARCA: ROPA ACTIVE
CLASSE: 25
ESPECIFICAÇÕES: Teste
TITULAR: Indústria ROPA Ltda

ACME Indústrias, doravante denominada REQUERENTE, vêm, respeitosamente, à presença de Vossa Senhoria, através de seu procurador firmatário, com base nos artigos 142, III, 143 e 144 da Lei n.º 9.279/96, apresentar pedido de CADUCIDADE da marca “ROPA ACTIVE”, registro n.º BR1234567/2025, pelos fatos e fundamentos que passa a expor:

A Requerente é titular da marca mista/nominativa/figurativa “ACME SPORT”, depositada no INPI em 05/06/2018, para identificar serviços/produtos compreendidos na classe 25.

As Diretrizes de Análise de Marcas do INPI dispõem sobre as condições para caracterização do legítimo interesse dos requerentes de pedido de caducidade, quais sejam: direitos já adquiridos, expectativa de direitos ou interesse em depositar sinal idêntico ou semelhante.

Neste sentido, resta justificado o legítimo interesse da Requerente, fundado na expectativa de direito de obter o registro da marca “ACME SPORT” de sua titularidade, razão pela qual vem, pelo presente, requerer a CADUCIDADE do registro anterior impeditivo da Requerida, processo n° BR1234567/2025.

O presente pedido de caducidade tem por objetivo fazer com que a Requerida comprove o uso legítimo e contínuo da marca “ROPA ACTIVE” no território brasileiro para identificar os serviços mencionados, na exata forma constante no Certificado de Registro, sob pena de ser declarada a sua caducidade, com a consequente extinção do Registro, conforme dispõem os arts. 143, I e II da LPI.

Em não sendo comprovado o uso da marca nos últimos cinco anos de forma lícita e ininterrupta pela titular, conforme consta no Certificado de Registro, requer-se que seja julgado procedente o presente pedido e, conseguintemente, declarado extinto o registro da marca em referência, nos termos do art. 142, III da LPI.

Nestes termos,
Pede e espera deferimento.

Porto Alegre, [DATA_ATUAL].
---

Agora, gere uma petição de caducidade igual ao modelo acima, mas usando os seguintes dados:

REGISTRO Nº: {dados.registro_requerida}
MARCA: {dados.marca_requerida}
CLASSE: {dados.classe_requerida}
ESPECIFICAÇÕES: {dados.especificacoes_requerida}
TITULAR: {dados.titular_requerida}
PROCESSO: {dados.processo_requerida}

DADOS DO CLIENTE:
NOME CLIENTE: {dados.nome_cliente}
MARCA CLIENTE: {dados.marca_cliente}
DATA DEPÓSITO: {dados.data_deposito_cliente}
CLASSE CLIENTE: {dados.classe_cliente}

Use essas informações para elaborar uma petição de caducidade completa e fundamentada conforme as normas do INPI.

A resposta deve ser o texto completo da petição, seguindo o padrão do modelo.
IMPORTANTE: Substitua [DATA_ATUAL] por: Porto Alegre, {data_atual}
"""
        resposta = self.openai_client.query_openai(prompt)
        
        # Converter dados Pydantic para dict para JSON storage
        dados_dict = dados.model_dump() if hasattr(dados, 'model_dump') else dados.dict()
        
        # Salva a petição no banco e retorna o objeto salvo
        peticao = self.repository.salvar_peticao(
            tipo="caducidade",
            processo_contestado=dados.registro_requerida,
            marca_contestada=dados.marca_requerida,
            texto_peticao=resposta,
            dados_origem=dados_dict
        )
        return {"id": peticao.id, "peticao": resposta}

    def reenviar_caducidade(self, chat_id, prompt_anterior, observacoes):
        peticao = self.repository.buscar_peticao_por_id(chat_id)
        if not peticao:
            raise Exception("Petição não encontrada")
        
        # Criar prompt para IA integrar as observações na petição original
        prompt_integracao = f"""
Você recebeu uma petição de caducidade já elaborada e observações adicionais do usuário.
Sua tarefa é MANTER TODO O CONTEÚDO ORIGINAL da petição e INTEGRAR as observações de forma natural e inteligente.

PETIÇÃO ORIGINAL:
{prompt_anterior}

OBSERVAÇÕES DO USUÁRIO:
{observacoes}

INSTRUÇÕES:
1. PRESERVE todo o texto original da petição
2. Integre as observações adicionais de forma natural no texto
3. Adicione as novas informações nos locais apropriados da petição
4. Mantenha a estrutura jurídica e formal
5. NÃO remova nenhum conteúdo existente
6. Se as observações contradizem algo existente, adicione as informações como complementares

Retorne a petição completa e aprimorada.
"""
        
        resposta = self.openai_client.query_openai(prompt_integracao)
        
        # Atualizar a petição original com o novo texto
        self.repository.atualizar_texto_peticao(chat_id, resposta)
        
        # Salvar no histórico de chat
        self.repository.salvar_historico_chat(chat_id, observacoes, resposta)
        
        return resposta

    def gerar_oposicao(self, dados):
        data_atual = datetime.now().strftime("%d/%m/%Y")
        prompt = f"""
Elabore uma PETIÇÃO DE OPOSIÇÃO COMPLETA para o INPI, com base nos seguintes dados:

PROCESSO Nº: {dados.processo_contestado}
MARCA CONTESTADA: {dados.marca_contestada}
CLASSE: {dados.classe_contestada}
ESPECIFICAÇÕES: {dados.especificacao_contestada}
TITULAR: {dados.titular_contestado}
NÚMERO RPI: {dados.numero_rpi if dados.numero_rpi else "Não informado"}
DATA RPI: {dados.data_rpi if dados.data_rpi else "Não informada"}

DADOS DO OPOENTE:
NOME CLIENTE: {dados.nome_cliente}
MARCA ANTERIOR: {dados.marca_anterior if dados.marca_anterior else "Não informada"}
PROCESSO ANTERIOR: {dados.processo_anterior if dados.processo_anterior else "Não informado"}
CLASSE ANTERIOR: {dados.classe_anterior if dados.classe_anterior else "Não informada"}
PRODUTOS ANTERIOR: {dados.produtos_anterior if dados.produtos_anterior else "Não informados"}

ANÁLISE DE CONFLITO:
TIPO DE CONFLITO: {dados.tipo_conflito if dados.tipo_conflito else "Não especificado"}
TIPO DE REPRODUÇÃO: {dados.tipo_reproducao if dados.tipo_reproducao else "Não especificado"}
ANÁLISE MERCADOLÓGICA: {dados.analise_mercadologica if dados.analise_mercadologica else "Não especificada"}
PRECEDENTES: {dados.precedentes if dados.precedentes else "Não informados"}
COEXISTÊNCIA: {dados.coexistencia if dados.coexistencia else "Não especificada"}

A petição deve seguir a estrutura formal adequada, fundamentada nos artigos 158 a 165 da Lei 9.279/96, 
apresentando argumentos sólidos sobre anterioridade, confundibilidade e proteção dos direitos adquiridos.

LOCAL E DATA: Porto Alegre, {data_atual}
"""
        resposta = self.openai_client.query_openai(prompt)
        
        # Converter dados Pydantic para dict para JSON storage
        dados_dict = dados.model_dump() if hasattr(dados, 'model_dump') else dados.dict()
        
        peticao = self.repository.salvar_peticao(
            tipo="oposicao",
            processo_contestado=dados.processo_contestado,
            marca_contestada=dados.marca_contestada,
            texto_peticao=resposta,
            dados_origem=dados_dict
        )
        return {"id": peticao.id, "peticao": resposta}

    def reenviar_oposicao(self, chat_id, prompt_anterior, observacoes):
        peticao = self.repository.buscar_peticao_por_id(chat_id)
        if not peticao:
            raise Exception("Petição não encontrada")
        
        # Criar prompt para IA integrar as observações na petição original
        prompt_integracao = f"""
Você recebeu uma petição de oposição já elaborada e observações adicionais do usuário.
Sua tarefa é MANTER TODO O CONTEÚDO ORIGINAL da petição e INTEGRAR as observações de forma natural e inteligente.

PETIÇÃO ORIGINAL:
{prompt_anterior}

OBSERVAÇÕES DO USUÁRIO:
{observacoes}

INSTRUÇÕES:
1. PRESERVE todo o texto original da petição
2. Integre as observações adicionais de forma natural no texto
3. Adicione as novas informações nos locais apropriados da petição
4. Mantenha a estrutura jurídica e formal
5. NÃO remova nenhum conteúdo existente
6. Se as observações contradizem algo existente, adicione as informações como complementares

Retorne a petição completa e aprimorada.
"""
        
        resposta = self.openai_client.query_openai(prompt_integracao)
        
        # Atualizar a petição original com o novo texto
        self.repository.atualizar_texto_peticao(chat_id, resposta)
        
        # Salvar no histórico de chat
        self.repository.salvar_historico_chat(chat_id, observacoes, resposta)
        
        return resposta

    def gerar_nulidade(self, dados):
        data_atual = datetime.now().strftime("%d/%m/%Y")
        prompt = f"""
Elabore uma PETIÇÃO DE NULIDADE DE REGISTRO DE MARCA COMPLETA para o INPI, com base nos seguintes dados:

REGISTRO CONTESTADO Nº: {dados.numero_registro}
MARCA CONTESTADA: {dados.marca_registrada}
CLASSE: {dados.classe_registro}
ESPECIFICAÇÕES: {dados.especificacao_registro}
TITULAR: {dados.titular_registro}
DATA CONCESSÃO: {dados.data_concessao if dados.data_concessao else "Não informada"}
RPI CONCESSÃO: {dados.rpi_concessao if dados.rpi_concessao else "Não informada"}

MARCA ANTERIOR: {dados.marca_anterior if dados.marca_anterior else "Não informada"}
REGISTRO/PROCESSO ANTERIOR: {dados.registro_anterior if dados.registro_anterior else "Não informado"}
CLASSE ANTERIOR: {dados.classe_anterior if dados.classe_anterior else "Não informada"}
ESPECIFICAÇÃO ANTERIOR: {dados.especificacao_anterior if dados.especificacao_anterior else "Não informada"}
TITULAR ANTERIOR: {dados.titular_anterior if dados.titular_anterior else "Não informado"}
DATA DEPÓSITO ANTERIOR: {dados.data_deposito_anterior if dados.data_deposito_anterior else "Não informada"}

ANÁLISE COMPARATIVA:
- Tipo de Reprodução: {dados.tipo_reproducao if dados.tipo_reproducao else "Não especificada"}
- Elementos Similares: {dados.elementos_similares if dados.elementos_similares else "Não especificados"}
- Análise Visual: {dados.analise_visual if dados.analise_visual else "Não especificada"}
- Análise Fonética: {dados.analise_fonetica if dados.analise_fonetica else "Não especificada"}

ARGUMENTOS ADICIONAIS:
- Precedentes: {dados.precedentes if dados.precedentes else "Não informados"}
- Má-fé: {dados.ma_fe if dados.ma_fe else "Não especificada"}
- Danos ao Mercado: {dados.danos_mercado if dados.danos_mercado else "Não especificados"}
- Decisões Anteriores: {dados.decisoes_anteriores if dados.decisoes_anteriores else "Não informadas"}

A petição deve ser fundamentada nos artigos 165 a 175 da Lei 9.279/96, 
demonstrando os vícios que levam à nulidade do registro contestado.
Inclua fundamentos jurídicos sólidos, análise de anterioridade e colidência entre as marcas.

LOCAL E DATA: Porto Alegre, {data_atual}
"""
        resposta = self.openai_client.query_openai(prompt)
        
        # Converter dados Pydantic para dict para JSON storage
        dados_dict = dados.model_dump() if hasattr(dados, 'model_dump') else dados.dict()
        
        peticao = self.repository.salvar_peticao(
            tipo="nulidade",
            processo_contestado=dados.numero_registro,
            marca_contestada=dados.marca_registrada,
            texto_peticao=resposta,
            dados_origem=dados_dict
        )
        return {"id": peticao.id, "peticao": resposta}

    def reenviar_nulidade(self, chat_id, prompt_anterior, observacoes):
        peticao = self.repository.buscar_peticao_por_id(chat_id)
        if not peticao:
            raise Exception("Petição não encontrada")
        
        # Criar prompt para IA integrar as observações na petição original
        prompt_integracao = f"""
Você recebeu uma petição de nulidade já elaborada e observações adicionais do usuário.
Sua tarefa é MANTER TODO O CONTEÚDO ORIGINAL da petição e INTEGRAR as observações de forma natural e inteligente.

PETIÇÃO ORIGINAL:
{prompt_anterior}

OBSERVAÇÕES DO USUÁRIO:
{observacoes}

INSTRUÇÕES:
1. PRESERVE todo o texto original da petição
2. Integre as observações adicionais de forma natural no texto
3. Adicione as novas informações nos locais apropriados da petição
4. Mantenha a estrutura jurídica e formal
5. NÃO remova nenhum conteúdo existente
6. Se as observações contradizem algo existente, adicione as informações como complementares

Retorne a petição completa e aprimorada.
"""
        
        resposta = self.openai_client.query_openai(prompt_integracao)
        
        # Atualizar a petição original com o novo texto
        self.repository.atualizar_texto_peticao(chat_id, resposta)
        
        # Salvar no histórico de chat
        self.repository.salvar_historico_chat(chat_id, observacoes, resposta)
        
        return resposta

    def gerar_manifestacao_oposicao(self, dados):
        data_atual = datetime.now().strftime("%d/%m/%Y")
        prompt = f"""
Elabore uma MANIFESTAÇÃO À OPOSIÇÃO COMPLETA para o INPI, com base nos seguintes dados:

PROCESSO Nº: {dados.processo_numero}
MARCA: {dados.processo_marca}
CLASSE: {dados.processo_classe}
ESPECIFICAÇÕES: {dados.processo_especificacao}
TITULAR: {dados.processo_titular}

DADOS DA OPOSIÇÃO:
RPI: {dados.oposicao_rpi}
DATA RPI: {dados.oposicao_data_rpi or 'Data não informada'}
OPOENTE: {dados.oposicao_opoente}
FUNDAMENTO: {dados.oposicao_fundamento}

MARCA ANTERIOR ALEGADA: {dados.marca_anterior}
PROCESSO ANTERIOR: {dados.processo_anterior}
TITULAR ANTERIOR: {dados.titular_anterior}

ARGUMENTAÇÃO:
FUNDAMENTO LEGAL: {dados.fundamento_legal}
ANÁLISE DE DISTINTIVIDADE: {dados.analise_distintividade}
COEXISTÊNCIA NO MERCADO: {dados.coexistencia_mercado}
PRECEDENTES: {dados.precedentes}

A manifestação deve refutar os argumentos da oposição, demonstrando a legitimidade do registro pleiteado.

LOCAL E DATA: Porto Alegre, {data_atual}
"""
        resposta = self.openai_client.query_openai(prompt)
        peticao = self.repository.salvar_peticao(
            tipo="manifestacao_oposicao",
            processo_contestado=dados.processo_numero,
            marca_contestada=dados.processo_marca,
            texto_peticao=resposta
        )
        return {"id": peticao.id, "peticao": resposta}

    def reenviar_manifestacao_oposicao(self, chat_id, prompt_anterior, observacoes):
        peticao = self.repository.buscar_peticao_por_id(chat_id)
        if not peticao:
            raise Exception("Petição não encontrada")
        
        # Criar prompt para IA integrar as observações na petição original
        prompt_integracao = f"""
Você recebeu uma manifestação à oposição já elaborada e observações adicionais do usuário.
Sua tarefa é MANTER TODO O CONTEÚDO ORIGINAL da petição e INTEGRAR as observações de forma natural e inteligente.

MANIFESTAÇÃO ORIGINAL:
{prompt_anterior}

OBSERVAÇÕES DO USUÁRIO:
{observacoes}

INSTRUÇÕES:
1. PRESERVE todo o texto original da manifestação
2. Integre as observações adicionais de forma natural no texto
3. Adicione as novas informações nos locais apropriados
4. Mantenha a estrutura jurídica e formal
5. NÃO remova nenhum conteúdo existente
6. Se as observações contradizem algo existente, adicione as informações como complementares

Retorne a manifestação completa e aprimorada.
"""
        
        resposta = self.openai_client.query_openai(prompt_integracao)
        
        # Atualizar a petição original com o novo texto
        self.repository.atualizar_texto_peticao(chat_id, resposta)
        
        # Salvar no histórico de chat
        self.repository.salvar_historico_chat(chat_id, observacoes, resposta)
        
        return resposta

    def gerar_manifestacao_recurso(self, dados):
        data_atual = datetime.now().strftime("%d/%m/%Y")
        prompt = f"""
Elabore uma MANIFESTAÇÃO AO RECURSO COMPLETA para o INPI, com base nos seguintes dados:

PROCESSO PRINCIPAL Nº: {dados.processo_numero}
MARCA: {dados.processo_marca}
CLASSE: {dados.processo_classe}
ESPECIFICAÇÕES: {dados.processo_especificacao}
TITULAR: {dados.processo_titular}

DADOS DO RECURSO:
RECURSO Nº: {dados.recurso_numero}
DATA: {dados.recurso_data or 'Data não informada'}
RECORRENTE: {dados.recorrente}
FUNDAMENTO: {dados.recurso_fundamento}

MARCA ANTERIOR (se aplicável): {dados.marca_anterior}
PROCESSO ANTERIOR: {dados.processo_anterior}
TITULAR ANTERIOR: {dados.titular_anterior}

ARGUMENTAÇÃO:
FUNDAMENTO LEGAL: {dados.fundamento_legal}
ANÁLISE DO MÉRITO: {dados.analise_merito}
PRECEDENTES: {dados.precedentes_jurisprudencia}
ARGUMENTOS TÉCNICOS: {dados.argumentos_tecnicos}

A manifestação deve contestar os argumentos do recurso, mantendo a decisão administrativa.

LOCAL E DATA: Porto Alegre, {data_atual}
"""
        resposta = self.openai_client.query_openai(prompt)
        peticao = self.repository.salvar_peticao(
            tipo="manifestacao_recurso",
            processo_contestado=dados.processo_numero,
            marca_contestada=dados.processo_marca,
            texto_peticao=resposta
        )
        return {"id": peticao.id, "peticao": resposta}

    def reenviar_manifestacao_recurso(self, chat_id, prompt_anterior, observacoes):
        peticao = self.repository.buscar_peticao_por_id(chat_id)
        if not peticao:
            raise Exception("Petição não encontrada")
        
        # Criar prompt para IA integrar as observações na petição original
        prompt_integracao = f"""
Você recebeu uma manifestação ao recurso já elaborada e observações adicionais do usuário.
Sua tarefa é MANTER TODO O CONTEÚDO ORIGINAL da petição e INTEGRAR as observações de forma natural e inteligente.

MANIFESTAÇÃO ORIGINAL:
{prompt_anterior}

OBSERVAÇÕES DO USUÁRIO:
{observacoes}

INSTRUÇÕES:
1. PRESERVE todo o texto original da manifestação
2. Integre as observações adicionais de forma natural no texto
3. Adicione as novas informações nos locais apropriados
4. Mantenha a estrutura jurídica e formal
5. NÃO remova nenhum conteúdo existente
6. Se as observações contradizem algo existente, adicione as informações como complementares

Retorne a manifestação completa e aprimorada.
"""
        
        resposta = self.openai_client.query_openai(prompt_integracao)
        
        # Atualizar a petição original com o novo texto
        self.repository.atualizar_texto_peticao(chat_id, resposta)
        
        # Salvar no histórico de chat
        self.repository.salvar_historico_chat(chat_id, observacoes, resposta)
        
        return resposta

    def gerar_recurso_indeferimento(self, dados):
        data_atual = datetime.now().strftime("%d/%m/%Y")
        prompt = f"""
Elabore um RECURSO CONTRA INDEFERIMENTO COMPLETO para o INPI, com base nos seguintes dados:

PROCESSO INDEFERIDO Nº: {dados.processo_numero}
MARCA: {dados.processo_marca}
CLASSE: {dados.processo_classe}
ESPECIFICAÇÕES: {dados.processo_especificacao}
TITULAR: {dados.processo_titular}
DATA INDEFERIMENTO: {dados.data_indeferimento or 'Data não informada'}

MOTIVO DO INDEFERIMENTO: {dados.motivo_indeferimento}
ARTIGO LEGAL: {dados.artigo_legal}
FUNDAMENTAÇÃO INPI: {dados.fundamentacao_inpi}

MARCA COLIDENTE (se aplicável): {dados.marca_colidente}
PROCESSO COLIDENTE: {dados.processo_colidente}
TITULAR COLIDENTE: {dados.titular_colidente}

ARGUMENTAÇÃO DO RECURSO:
CONTRA-ARGUMENTAÇÃO: {dados.contra_argumentacao}
DISTINTIVIDADE: {dados.distintividade}
AUSÊNCIA DE CONFUNDIBILIDADE: {dados.ausencia_confundibilidade}
COEXISTÊNCIA PACÍFICA: {dados.coexistencia_pacifica}
PRECEDENTES: {dados.precedentes}
DOUTRINA: {dados.doutrina}

O recurso deve ser fundamentado no art. 212 da Lei 9.279/96, contestando tecnicamente o indeferimento.

LOCAL E DATA: Porto Alegre, {data_atual}
"""
        resposta = self.openai_client.query_openai(prompt)
        peticao = self.repository.salvar_peticao(
            tipo="recurso_indeferimento",
            processo_contestado=dados.processo_numero,
            marca_contestada=dados.processo_marca,
            texto_peticao=resposta
        )
        return {"id": peticao.id, "peticao": resposta}

    def reenviar_recurso_indeferimento(self, chat_id, prompt_anterior, observacoes):
        peticao = self.repository.buscar_peticao_por_id(chat_id)
        if not peticao:
            raise Exception("Petição não encontrada")
        
        # Criar prompt para IA integrar as observações na petição original
        prompt_integracao = f"""
Você recebeu um recurso contra indeferimento já elaborado e observações adicionais do usuário.
Sua tarefa é MANTER TODO O CONTEÚDO ORIGINAL da petição e INTEGRAR as observações de forma natural e inteligente.

RECURSO ORIGINAL:
{prompt_anterior}

OBSERVAÇÕES DO USUÁRIO:
{observacoes}

INSTRUÇÕES:
1. PRESERVE todo o texto original do recurso
2. Integre as observações adicionais de forma natural no texto
3. Adicione as novas informações nos locais apropriados
4. Mantenha a estrutura jurídica e formal
5. NÃO remova nenhum conteúdo existente
6. Se as observações contradizem algo existente, adicione as informações como complementares

Retorne o recurso completo e aprimorado.
"""
        
        resposta = self.openai_client.query_openai(prompt_integracao)
        
        # Atualizar a petição original com o novo texto
        self.repository.atualizar_texto_peticao(chat_id, resposta)
        
        # Salvar no histórico de chat
        self.repository.salvar_historico_chat(chat_id, observacoes, resposta)
        
        return resposta

    def gerar_peticao_caducidade(self, dados):
        data_atual = datetime.now().strftime("%d/%m/%Y")
        prompt = f"""
Elabore uma PETIÇÃO DE CADUCIDADE DE REGISTRO COMPLETA para o INPI, com base nos seguintes dados:

REGISTRO Nº: {dados.registro_requerida}
MARCA: {dados.marca_requerida}
CLASSE: {dados.classe_requerida}
ESPECIFICAÇÕES: {dados.especificacoes_requerida}
TITULAR: {dados.titular_requerida}
DATA CONCESSÃO: {getattr(dados, 'data_concessao', 'Data não informada')}

PERÍODO SEM USO: {getattr(dados, 'periodo_nao_uso', 'Período não informado')}
INÍCIO DO NÃO USO: {getattr(dados, 'data_inicio_nao_uso', 'Data não informada')}

PESQUISAS REALIZADAS:
MERCADO: {dados.pesquisa_mercado}
INTERNET: {dados.pesquisa_internet}
ÓRGÃOS: {dados.consulta_orgaos}

JUSTIFICATIVAS:
AUSÊNCIA DE USO: {dados.ausencia_uso_efetivo}
FALTA DE COMPROVAÇÃO: {dados.falta_comprovacao}
INTERESSE DO REQUERENTE: {dados.interesse_requerente}

MARCA DO REQUERENTE: {dados.marca_requerente}
PROCESSO DO REQUERENTE: {dados.processo_requerente}

A petição deve ser fundamentada nos arts. 142, III, 143 e 144 da Lei 9.279/96.

LOCAL E DATA: Porto Alegre, {data_atual}
"""
        resposta = self.openai_client.query_openai(prompt)
        peticao = self.repository.salvar_peticao(
            tipo="caducidade",
            processo_contestado=dados.registro_requerida,
            marca_contestada=dados.marca_requerida,
            texto_peticao=resposta
        )
        return {"id": peticao.id, "peticao": resposta}

    def reenviar_peticao_caducidade(self, chat_id, prompt_anterior, observacoes):
        peticao = self.repository.buscar_peticao_por_id(chat_id)
        if not peticao:
            raise Exception("Petição não encontrada")
        
        # Criar prompt para IA integrar as observações na petição original
        prompt_integracao = f"""
Você recebeu uma petição de caducidade já elaborada e observações adicionais do usuário.
Sua tarefa é MANTER TODO O CONTEÚDO ORIGINAL da petição e INTEGRAR as observações de forma natural e inteligente.

PETIÇÃO ORIGINAL:
{prompt_anterior}

OBSERVAÇÕES DO USUÁRIO:
{observacoes}

INSTRUÇÕES:
1. PRESERVE todo o texto original da petição
2. Integre as observações adicionais de forma natural no texto
3. Adicione as novas informações nos locais apropriados da petição
4. Mantenha a estrutura jurídica e formal
5. NÃO remova nenhum conteúdo existente
6. Se as observações contradizem algo existente, adicione as informações como complementares

Retorne a petição completa e aprimorada.
"""
        
        resposta = self.openai_client.query_openai(prompt_integracao)
        
        # Atualizar a petição original com o novo texto
        self.repository.atualizar_texto_peticao(chat_id, resposta)
        
        # Salvar no histórico de chat
        self.repository.salvar_historico_chat(chat_id, observacoes, resposta)
        
        return resposta

    def gerar_contrarazao_nulidade(self, dados):
        data_atual = datetime.now().strftime("%d/%m/%Y")
        prompt = f"""
Elabore uma CONTRARAZÃO À NULIDADE COMPLETA para o INPI, com base nos seguintes dados:

PROCESSO DE NULIDADE: {dados.processo_nulidade}
MARCA CONTESTADA: {dados.processo_marca_contestada}
REGISTRO CONTESTADO: {dados.registro_contestado}
CLASSE: {dados.classe_contestada}
ESPECIFICAÇÕES: {dados.especificacao_contestada}
TITULAR: {dados.titular_contestado}

REQUERENTE DA NULIDADE: {dados.requerente_nulidade}
DATA PEDIDO: {dados.data_pedido_nulidade or 'Data não informada'}
FUNDAMENTOS ALEGADOS: {dados.fundamentos_nulidade}

MARCA ANTERIOR ALEGADA: {dados.marca_anterior_alegada or 'Não informada'}

A contrarazão deve refutar os argumentos de nulidade, defendendo a validade do registro.

LOCAL E DATA: Porto Alegre, {data_atual}
"""
        resposta = self.openai_client.query_openai(prompt)
        
        # Criar dados de origem para salvar detalhes
        dados_origem = {
            'cliente': dados.cliente,
            'classe': dados.classe,
            'titular': dados.titular or '',
            'especificacoes': dados.especificacoes or '',
            'numero': dados.numero or '',
            'data': dados.data or '',
            'marca_terceiro': dados.marca_terceiro or '',
            'processo_terceiro': dados.processo_terceiro or '',
            'marca_requerida': dados.marca_requerida or '',
            'processo_requerido': dados.processo_requerido or '',
            'marca_cliente': dados.marca_cliente or ''
        }
        
        peticao = self.repository.salvar_peticao(
            tipo="contrarazao_nulidade",
            processo_contestado=dados.processo_nulidade,
            marca_contestada=dados.processo_marca_contestada,
            texto_peticao=resposta,
            dados_origem=dados_origem
        )
        return {"id": peticao.id, "peticao": resposta}

    def reenviar_contrarazao_nulidade(self, chat_id, prompt_anterior, observacoes):
        peticao = self.repository.buscar_peticao_por_id(chat_id)
        if not peticao:
            raise Exception("Petição não encontrada")
        
        # Criar prompt para IA integrar as observações na petição original
        prompt_integracao = f"""
Você recebeu uma contrarrazão à nulidade já elaborada e observações adicionais do usuário.
Sua tarefa é MANTER TODO O CONTEÚDO ORIGINAL da petição e INTEGRAR as observações de forma natural e inteligente.

CONTRARRAZÃO ORIGINAL:
{prompt_anterior}

OBSERVAÇÕES DO USUÁRIO:
{observacoes}

INSTRUÇÕES:
1. PRESERVE todo o texto original da contrarrazão
2. Integre as observações adicionais de forma natural no texto
3. Adicione as novas informações nos locais apropriados
4. Mantenha a estrutura jurídica e formal
5. NÃO remova nenhum conteúdo existente
6. Se as observações contradizem algo existente, adicione as informações como complementares

Retorne a contrarrazão completa e aprimorada.
"""
        
        resposta = self.openai_client.query_openai(prompt_integracao)
        
        # Atualizar a petição original com o novo texto
        self.repository.atualizar_texto_peticao(chat_id, resposta)
        
        # Salvar no histórico de chat
        self.repository.salvar_historico_chat(chat_id, observacoes, resposta)
        
        return resposta