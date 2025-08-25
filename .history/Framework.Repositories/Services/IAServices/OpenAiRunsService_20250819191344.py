from typing import List, Optional, Any
from datetime import datetime
class OpenAiRunsService:
    """
    Python class equivalent to the C# OpenAiRunsService.
    Likely a repository implementation or Unit of Work.
    """
    # Assuming dependency injection for parameters
    def __init__(self, http_client: HttpClient, configuration: IConfiguration):
        self.http_client = http_client
        self.configuration = configuration

    # TODO: Translate logic from C# method 'CreateRunAsync'
    async def create_run_async(self, messages: List_ChatMessage) -> string: # Basic return type mapping
        pass

    # TODO: Translate logic from C# method 'AddMessageToRunAsync'
    async def add_message_to_run_async(self, run_id: string, message: ChatMessage) -> string: # Basic return type mapping
        pass
