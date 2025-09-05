from fastapi import HTTPException
import openai
from app.config.settings import settings

class OpenAIClient:
    def __init__(self):
        openai.api_key = settings.OPENAI_API_KEY

    async def query_openai(self, prompt: str) -> str:
        try:
            response = openai.ChatCompletion.create(
                model="gpt-3.5-turbo",
                messages=[{"role": "user", "content": prompt}]
            )
            return response['choices'][0]['message']['content']
        except Exception as e:
            raise HTTPException(status_code=500, detail=str(e))