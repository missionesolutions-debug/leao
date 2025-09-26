class OracleService:
    def __init__(self, openai_client):
        self.openai_client = openai_client

    def query_openai(self, prompt: str) -> str:
        # Contexto especializado para propriedade intelectual
        system_prompt = """Você é um assistente especializado em Propriedade Intelectual e Direito Marcário brasileiro. 
        Suas respostas devem ser:
        - Baseadas na legislação brasileira (LPI - Lei 9.279/96)
        - Práticas e objetivas
        - Focadas em marcas, patentes e direitos autorais
        - Com linguagem jurídica apropriada mas acessível
        
        Se a pergunta não for relacionada a propriedade intelectual, educadamente redirecione para temas da área."""
        
        enhanced_prompt = f"{system_prompt}\n\nPergunta: {prompt}"
        
        response = self.openai_client.query_openai(
            prompt=enhanced_prompt,
            model="gpt-3.5-turbo",
            max_tokens=300
        )
        return response

    def format_response(self, response: dict) -> str:
        if 'choices' in response and len(response['choices']) > 0:
            return response['choices'][0]['text'].strip()
        return "No response from OpenAI."