from app.models.peticao_model import Peticao
from app.models.peticao_detalhes_model import PeticaoDetalhes
from app.models.chat_historico_model import ChatHistorico

class PeticaoRepository:
    def __init__(self, db):
        self.db = db

    def salvar_peticao(self, tipo, processo_contestado, marca_contestada, texto_peticao, dados_origem=None):
        # Criar a petição principal
        peticao = Peticao(
            tipo=tipo,
            processo_contestado=processo_contestado,
            marca_contestada=marca_contestada,
            texto_peticao=texto_peticao
        )
        self.db.add(peticao)
        self.db.commit()
        self.db.refresh(peticao)
        
        # Salvar detalhes se fornecidos
        if dados_origem:
            detalhes = PeticaoDetalhes(
                peticao_id=peticao.id,
                dados_origem=dados_origem,
                numero_processo=dados_origem.get('numero_processo') or dados_origem.get('processo_contestado'),
                numero_registro=dados_origem.get('numero_registro') or dados_origem.get('registro_numero'),
                marca_registrada=dados_origem.get('marca_registrada') or dados_origem.get('registro_marca') or dados_origem.get('marca_contestada'),
                classe_marca=dados_origem.get('classe_registro') or dados_origem.get('registro_classe') or dados_origem.get('classe_contestada'),
                titular_registro=dados_origem.get('titular_registro') or dados_origem.get('registro_titular') or dados_origem.get('titular_contestado')
            )
            self.db.add(detalhes)
            self.db.commit()
        
        return peticao

    def salvar_historico_chat(self, peticao_id, prompt_usuario, resposta_ia):
        historico = ChatHistorico(
            peticao_id=peticao_id,
            prompt_usuario=prompt_usuario,
            resposta_ia=resposta_ia
        )
        self.db.add(historico)
        self.db.commit()
        return historico

    def buscar_peticao_por_id(self, peticao_id: int):
        return self.db.query(Peticao).filter(Peticao.id == peticao_id).first()
    
    def buscar_historico_chat(self, peticao_id: int):
        return self.db.query(ChatHistorico).filter(ChatHistorico.peticao_id == peticao_id).order_by(ChatHistorico.created_at).all()

    def listar_todas_peticoes(self):
        """Lista todas as petições ordenadas por data de criação"""
        return self.db.query(Peticao).order_by(Peticao.created_at.desc()).all()

    def listar_por_tipo(self, tipo: str):
        """Lista petições de um tipo específico"""
        return self.db.query(Peticao).filter(Peticao.tipo == tipo).order_by(Peticao.created_at.desc()).all()

    def atualizar_texto_peticao(self, peticao_id: int, novo_texto: str):
        """Atualiza o texto da petição com a versão editada"""
        try:
            peticao = self.db.query(Peticao).filter(Peticao.id == peticao_id).first()
            if peticao:
                peticao.texto_peticao = novo_texto
                self.db.commit()
                self.db.refresh(peticao)
                return peticao
            return None
        except Exception as e:
            self.db.rollback()
            raise e

    def excluir_peticao(self, peticao_id: int):
        """Exclui uma petição e seus dados relacionados"""
        try:
            # Excluir detalhes relacionados
            self.db.query(PeticaoDetalhes).filter(PeticaoDetalhes.peticao_id == peticao_id).delete()
            # Excluir histórico de chat relacionado
            self.db.query(ChatHistorico).filter(ChatHistorico.peticao_id == peticao_id).delete()
            # Excluir a petição
            peticao = self.db.query(Peticao).filter(Peticao.id == peticao_id).first()
            if peticao:
                self.db.delete(peticao)
                self.db.commit()
                return True
            return False
        except Exception as e:
            self.db.rollback()
            raise e