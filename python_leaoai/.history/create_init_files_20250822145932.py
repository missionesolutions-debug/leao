import os

# Caminho raiz do seu projeto (onde está main.py)
root_path = os.path.dirname(os.path.abspath(__file__))

print(f"Verificando diretórios em: {root_path}")

# Percorrer todas as pastas dentro do root_path recursivamente
for dirpath, dirnames, filenames in os.walk(root_path):
    init_file = os.path.join(dirpath, '__init__.py')
    if not os.path.exists(init_file):
        with open(init_file, 'w', encoding='utf-8') as f:
            f.write("# Este arquivo torna o diretório um pacote Python\n")
        print(f"Criado: {init_file}")
    else:
        print(f"Já existe: {init_file}")

print("Processo concluído! Todos os diretórios agora têm __init__.py")
