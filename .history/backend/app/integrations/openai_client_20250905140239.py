from fastapi import HTTPException
import openai
import asyncio
from app.config.settings import settings

class OpenAIClient:
    def __init__(self):
        openai.api_key = settings.OPENAI_API_KEY

    async def query_openai(self, prompt: str, model: str = "gpt-3.5-turbo", max_tokens: int = 150) -> str:
        try:
            loop = asyncio.get_event_loop()
            response = await loop.run_in_executor(
                None,
                lambda: openai.ChatCompletion.create(
                    model=model,
                    messages=[{"role": "user", "content": prompt}],
                    max_tokens=max_tokens
                )
            )
            return response['choices'][0]['message']['content']
        except Exception as e:
            raise HTTPException(status_code=500, detail=str(e))