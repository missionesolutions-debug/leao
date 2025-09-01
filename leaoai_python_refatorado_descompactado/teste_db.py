import mysql.connector
from mysql.connector import Error

def test_connection():
    try:
        connection = mysql.connector.connect(
            host="ph103.peopleshostshared.com",
            user="ieijcomb_LeaoAdv",
            password="ZEDZOTHus56XIq1A",
            database="ieijcomb_LeaoAdv",
            port=3306
        )

        if connection.is_connected():
            print("✅ Conexão bem-sucedida ao MySQL!")
            cursor = connection.cursor()
            cursor.execute("SELECT DATABASE();")
            record = cursor.fetchone()
            print(f"Banco de dados atual: {record[0]}")

    except Error as e:
        print(f"❌ Erro na conexão: {e}")

    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            connection.close()
            print("🔌 Conexão encerrada.")

if __name__ == "__main__":
    test_connection()
