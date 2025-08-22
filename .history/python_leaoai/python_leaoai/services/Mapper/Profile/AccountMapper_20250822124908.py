from datetime import datetime

# Model de usuário equivalente ao C# Usuario
class Usuario:
    def __init__(self,
                 nome: str = None,
                 login: str = None,
                 email: str = None,
                 password: str = None,
                 role_gate: str = "Student",
                 ativo: bool = True,
                 excluido: bool = False,
                 destaque: bool = False,
                 phone: str = None,
                 data_criacao: datetime = None,
                 genero: str = None,
                 logradouro: str = None,
                 estado: str = None,
                 cidade: str = None,
                 bairro: str = None,
                 complemento: str = None,
                 numero: str = None,
                 data_nascimento: datetime = None,
                 cep: str = None,
                 cpf: str = None):
        self.nome = nome
        self.login = login
        self.email = email
        self.password = password
        self.role_gate = role_gate
        self.ativo = ativo
        self.excluido = excluido
        self.destaque = destaque
        self.phone = phone
        self.data_criacao = data_criacao
        self.genero = genero
        self.logradouro = logradouro
        self.estado = estado
        self.cidade = cidade
        self.bairro = bairro
        self.complemento = complemento
        self.numero = numero
        self.data_nascimento = data_nascimento
        self.cep = cep
        self.cpf = cpf


# DTO equivalente ao C# CreateAccountResponse
class CreateAccountResponse:
    def __init__(self,
                 nome: str = None,
                 email: str = None,
                 password: str = None,
                 phone: str = None,
                 genero: str = None,
                 logradouro: str = None,
                 estado: str = None,
                 cidade: str = None,
                 bairro: str = None,
                 complemento: str = None,
                 numero: str = None,
                 data_nascimento: datetime = None,
                 cep: str = None,
                 cpf: str = None):
        self.nome = nome
        self.email = email
        self.password = password
        self.phone = phone
        self.genero = genero
        self.logradouro = logradouro
        self.estado = estado
        self.cidade = cidade
        self.bairro = bairro
        self.complemento = complemento
        self.numero = numero
        self.data_nascimento = data_nascimento
        self.cep = cep
        self.cpf = cpf


class AccountMapper:

    @staticmethod
    def mapping_user(model: CreateAccountResponse) -> Usuario:
        return Usuario(
            nome=model.nome,
            login=model.email,
            email=model.email,
            password=AccountMapper.hash_password(model.password),
            role_gate="Student",
            ativo=True,
            excluido=False,
            destaque=False,
            phone=model.phone,
            data_criacao=datetime.now(),
            genero=model.genero,
            logradouro=model.logradouro,
            estado=model.estado,
            cidade=model.cidade,
            bairro=model.bairro,
            complemento=model.complemento,
            numero=model.numero,
            data_nascimento=model.data_nascimento,
            cep=model.cep,
            cpf=model.cpf
        )

    @staticmethod
    def hash_password(password: str) -> str:
        # Aqui você pode implementar o hashing real, por exemplo com bcrypt
        # import bcrypt
        # return bcrypt.hashpw(password.encode(), bcrypt.gensalt()).decode()
        return password
