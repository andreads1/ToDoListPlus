import React from 'react';
import ReactDOM from 'react-dom';
import './Index.css';

export default class Index
    extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            item: {
                id: "",
                tarefa: ""
            },
            filtro: "",
            itens: [],
            editando: false
        }

    }

    componentDidMount = () => {
        this.obterTodos();
    }

    componentDidUpdate = () => {

    }

    adicionar = () => {
        HTTPClient.post("/ToDoListPlus/Salvar", this.state.item)
            .then((respostaJSON) => respostaJSON.json())
            .then((respostaObj) => {
                alert(respostaObj.msg);

                if (respostaObj.sucesso) {

                    if (this.state.editando)
                        this.obter(this.state.item.id);
                    else {
                        this.obterTodos();
                    }

                    this.setState({
                        ...this.state,
                        item: {
                            id: "",
                            tarefa: "",
                        },
                        editando: false
                    });
                }

            })
            .catch((err) => {
                alert("Erro ao adicionar item");
                console.log(err.message);
            });

    }

    obterTodos = () => {
        //debugger;
        HTTPClient.get("/ToDoListPlus/ObterTodos")
            .then((respostaJSON) => respostaJSON.json())
            .then((respostaObj) => {
                this.setState({
                    ...this.state,
                    itens: respostaObj
                });
            })
            .catch((err) => {
                alert("Erro ao obter todos os itens");
                console.log(err.message);
            });
    }

    obter = (id) => {
        let url = "/ToDoListPlus/ObterPorId?id=" + id;

        HTTPClient.get(url)
            .then(respostaJSON => respostaJSON.json())
            .then(resposta => {
                let itens = this.state.itens;
                let index = itens.findIndex(item => item.id === id);

                if (index > -1) {
                    itens[index].id = resposta.id;
                    itens[index].tarefa = resposta.tarefa;

                    this.setState({
                        ...this.state,
                        itens
                    })
                }

            })
            .catch((err) => {
                alert("Erro ao buscar item");
                console.log(err.message);
            })
    }

    excluir = (i) => {
        //debugger;

        if (!window.confirm(`Deseja excluir ${i.tarefa}?`)) {
            return;
        }

        let url = "/ToDoListPlus/Excluir?id=" + i.id;

        HTTPClient.delete(url)
            .then(respostaJSON => respostaJSON.json())
            .then(resposta => {
                if (resposta.sucesso) {
                    let itens = this.state.itens;

                    let index = itens.findIndex(item => item.id === i.id);

                    if (index > -1) {
                        itens.splice(index, 1);
                        this.setState({
                            ...this.state,
                            itens
                        });
                    }
                }
                alert(resposta.msg);
            })
            .catch((err) => {
                alert("Erro ao excluir item!");
                console.log(err.message);
            });

    }


    editar = (i) => {
        //debugger;
        debugger;
        let url = "/ToDoListPlus/ObterPorId?id=" + i.id;
        HTTPClient.get(url)
            .then(respostaJSON => respostaJSON.json())
            .then(resposta => {
                this.setState({
                    ...this.state,
                    item: resposta,
                    editando: true
                })
            })
            .catch((err) => {
                alert("Erro ao editar o item!");
                console.log(err.message);
            });

    }

    filtrar = () => {
        //debugger;
        let pesquisar = this.state.filtro.toLowerCase();//texto a ser buscado

        let itens = this.state.itens;

        itens.forEach(i => {
            if (i.tarefa.toLowerCase().indexOf(pesquisar) > -1) {
                i.filtrado = true;
            }
            else
                i.filtrado = false;
        });

        this.setState({
            ...this.state,
            itens: itens
        })

    }

    render = () => {
        let saida =
            <>
                <div className="bloco">
                    <h1>To do List Plus</h1>
                    <br />

                    <label className="entradas">ID&nbsp;</label>
                    <input type="number"
                        value={this.state.item.id}
                        onChange={(e) => this.setState({
                            ...this.state,
                            item: {
                                ...this.state.item,
                                id: e.target.value
                            },
                        })}
                    />

                    <label className="entradas">&nbsp;&nbsp;&nbsp;Tarefa&nbsp;</label>
                    <input type="text"
                        value={this.state.item.tarefa}
                        onChange={(e) => this.setState({
                            ...this.state,
                            item: {
                                ...this.state.item,
                                tarefa: e.target.value
                            }
                        })}
                    />&nbsp;&nbsp;
                    <button type="button"
                        onClick={this.adicionar}>+</button>
                    <br />
                    <br />

                    <label>Filtro </label><br />
                    <input type="search"
                        value={this.state.filtro}
                        onChange={(e) => this.setState({
                            ...this.state,
                            filtro: e.target.value
                        }, () => { this.filtrar() }
                        )}
                    />
                    <br />
                    <br />
                    <table id="tabela">
                        <tr className="titulo">
                            <td><b>ID</b></td>
                            <td><b>Tarefa</b></td>
                            <td></td>
                            <td></td>
                        </tr>
                        {this.state.itens.map(i => {
                            if (i.filtrado === undefined || i.filtrado) {
                                return (
                                    <tr>
                                        <td>{i.id}</td>
                                        <td>{i.tarefa}</td>
                                        <td>
                                            <button type="button"
                                                onClick={() => this.excluir(i)}>Remover</button>
                                        </td>
                                        <td>
                                            <button type="button"
                                                onClick={() => this.editar(i)}>Editar</button>
                                        </td>
                                    </tr>

                                )
                            }
                        }
                        ) //
                        }
                    </table>
                </div>
            </>

        return saida;
    }
}

ReactDOM.render(<Index />, document.getElementById("root"));