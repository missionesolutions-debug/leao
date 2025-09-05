import openai
from app.config.settings import settings
from fastapi import HTTPException
class OpenAIClient:
    def __init__(self):
        self.client = openai.OpenAI(api_key=settings.OPENAI_API_KEY)

    async def query_openai(self, prompt: str, model: str = "gpt-3.5-turbo", max_tokens: int = 150) -> str:
        try:
            response = await self.client.chat.completions.create(
                model=model,
                messages=[{"role": "user", "content": prompt}],
                max_tokens=max_tokens
            )
            return response.choices[0].message.content
        except Exception as e:
            raise HTTPException(status_code=500, detail=str(e))