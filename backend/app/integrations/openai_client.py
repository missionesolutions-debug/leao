import openai
import time
import random
from app.config.settings import settings
from fastapi import HTTPException
class OpenAIClient:
    def __init__(self):
        self.client = openai.OpenAI(
            api_key=settings.OPENAI_API_KEY,
            timeout=60.0  # Timeout de 60 segundos
        )

    def query_openai(self, prompt: str, model: str = "gpt-3.5-turbo", max_tokens: int = 1500) -> str:
        max_retries = 3
        base_delay = 1
        
        for attempt in range(max_retries):
            try:
                response = self.client.chat.completions.create(
                    model=model,
                    messages=[{"role": "user", "content": prompt}],
                    max_tokens=max_tokens,
                    timeout=60  # Timeout específico para esta request
                )
                return response.choices[0].message.content
                
            except openai.RateLimitError as e:
                if attempt < max_retries - 1:
                    # Exponential backoff com jitter
                    delay = base_delay * (2 ** attempt) + random.uniform(0, 1)
                    print(f"Rate limit atingido. Tentativa {attempt + 1}/{max_retries}. Aguardando {delay:.2f}s...")
                    time.sleep(delay)
                    continue
                else:
                    raise HTTPException(
                        status_code=429, 
                        detail="Rate limit da OpenAI excedido. Tente novamente em alguns minutos."
                    )
                    
            except openai.APITimeoutError as e:
                if attempt < max_retries - 1:
                    delay = base_delay * (2 ** attempt)
                    print(f"Timeout na API. Tentativa {attempt + 1}/{max_retries}. Aguardando {delay}s...")
                    time.sleep(delay)
                    continue
                else:
                    raise HTTPException(
                        status_code=408, 
                        detail="Timeout na geração da petição. Tente novamente."
                    )
                    
            except Exception as e:
                if attempt < max_retries - 1:
                    delay = base_delay * (2 ** attempt)
                    print(f"Erro inesperado: {str(e)}. Tentativa {attempt + 1}/{max_retries}. Aguardando {delay}s...")
                    time.sleep(delay)
                    continue
                else:
                    raise HTTPException(
                        status_code=500, 
                        detail=f"Erro na geração da petição: {str(e)}"
                    )