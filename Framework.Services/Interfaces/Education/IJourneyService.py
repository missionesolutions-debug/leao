from abc import ABC, abstractmethod
from typing import List, Optional, Any
from datetime import datetime
class IJourneyService(ABC):
    """
    Python interface equivalent to the C# IJourneyService.
    """

    @abstractmethod
    def get_user_missions(self, email_address: string) -> UserMissionsDto: # Basic return type mapping
        pass # TODO: Translate logic from C# method 'GetUserMissions'
