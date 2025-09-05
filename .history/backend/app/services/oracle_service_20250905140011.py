class OracleService:
    def __init__(self, openai_client):
        self.openai_client = openai_client

    async def query_openai(self, prompt: str) -> dict:
        response = await self.openai_client.query_openai(
            model="gpt-3.5-turbo",
            prompt=prompt,
            max_tokens=150
        )
        return response

    def format_response(self, response: dict) -> str:
        if 'choices' in response and len(response['choices']) > 0:
            return response['choices'][0]['text'].strip()
        return "No response from OpenAI."