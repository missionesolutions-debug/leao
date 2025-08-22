from typing import List, Optional, Any
from datetime import datetime
from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey
from sqlalchemy.orm import relationship
class ProjectGroup:
    """
    Python class equivalent to the C# ProjectGroup.
    """
