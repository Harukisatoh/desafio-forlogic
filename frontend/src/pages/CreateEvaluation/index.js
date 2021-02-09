import React, { useEffect, useState } from 'react';
import Navbar from '../../Components/NavMenu';
import { Form, Button, ListGroup, Jumbotron, Card, Alert, Badge } from 'react-bootstrap';

export default function CreateEvaluation() {
    const [allClients, setAllClients] = useState([]);
    const [filteredClients, setFilteredClients] = useState([]);
    const [evaluationReferenceDate, setEvaluationReferenceDate] = useState();
    const [clientEvaluations, setClientEvaluations] = useState([]);
    const [showAlert, setShowAlert] = useState(false);
    const [responseBody, setResponseBody] = useState('');
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        fetch('http://localhost:50234/api/Client')
            .then(response => response.json())
            .then(json => {
                setAllClients(json);
                setFilteredClients(json);
            });
    }, []);

    function toggleClient(e, clientId) {
        e.preventDefault();

        if(clientEvaluations.findIndex((clientEvaluation) => clientEvaluation.clientId === clientId) !== -1) {
            setClientEvaluations((clientEvaluations) => {
                const newClientEvaluations = clientEvaluations.filter((clientEvaluation) => clientEvaluation.clientId !== clientId);

                return newClientEvaluations;
            });
        } else {
            setClientEvaluations((clientEvaluations) => [...clientEvaluations, { clientId: clientId, grade: '', reason: '' }]);
        }
    }

    function formatDate(dateString) {
        const date = new Date(dateString);
        const year = date.getFullYear();
        const month = date.getMonth() + 1;

        const formattedDate = `${month}/${year}`;
        return formattedDate;
    }

    function editEvaluationReferenceDate(date) {
        const selectedDate = new Date(date);

        const year = selectedDate.getUTCFullYear();
        const month = selectedDate.getUTCMonth() + 1;
        const formattedDate = `${year}-${month}-01`;
        
        setEvaluationReferenceDate(formattedDate);
    }

    function editEvaluation(clientId, field, value) {
        setClientEvaluations((clientEvaluations) => {
            const newClientEvaluations = clientEvaluations.map((clientEvaluation) => {
                if (clientId !== clientEvaluation.clientId) return clientEvaluation;
                const newClientEvaluation = { ...clientEvaluation, [field]: value };
                return (newClientEvaluation);
            })
            return newClientEvaluations
        });
    }

    function isClientSelected(clientId) {
        const clientEvaluationIndex = clientEvaluations.findIndex((clientEvaluation) => clientEvaluation.clientId === clientId);
    
        
        if(clientEvaluationIndex === -1) {
            return false;
        } else {
            return true;
        }
    }

    function filterClients(clientCompanyName) {
        console.log('filtrando...')
        setFilteredClients(() => {
            const filteredClients = allClients.filter((client) => client.ClientCompanyName.includes(clientCompanyName));

            return filteredClients;
        });
    }

    function calculateSelectedClientPercentage() {
        try {
            const selectedPercentage = clientEvaluations.length * 100 / allClients.length;
            return selectedPercentage.toPrecision(3);
        } catch (exception) {
            console.log(exception)
        }
    }

    function formSubmit(e) {
        e.preventDefault();
        
        if(clientEvaluations.length === 0) {
            setShowAlert(true);
            setResponseBody({ error: 'Selecione pelo menos um cliente que realizou a avaliação!' });
            return;
        }
        
        setLoading(true);
        fetch('http://localhost:50234/api/Form', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                evaluationReferenceDate,
                clientEvaluations
            })
        })
            .then(response => response.json())
            .then(json => {
                setResponseBody(json);
                setShowAlert(true);
                setLoading(false);
            });
    }

    return (
        <>
            <Navbar />
            <Jumbotron>
                <h1>Cadastro de Avaliação</h1>
                <Form autoComplete="off" onSubmit={formSubmit}>
                    <Form.Group controlId="formDate">
                        <Form.Label>Data da avaliação</Form.Label>
                        <Form.Control
                            required
                            type="date"
                            placeholder="Selecione a data em que a avaliação ocorreu"
                            onChange={(event) => editEvaluationReferenceDate(event.target.value)}
                        />
                        <Form.Text className="text-muted">
                        Selecione a data em que a avaliação ocorreu
                        </Form.Text>
                    </Form.Group>

                    <Form.Group controlId="formClientSelect">
                        <h4 style={{ marginBottom: 30, marginTop: 20 }}>Selecione os usuários que participaram</h4>
                        <ListGroup>
                            <ListGroup.Item style={{ display: 'flex' }}>
                                <Form.Control
                                    type="text"
                                    placeholder="Pesquisar cliente..."
                                    className="mr-sm-2"
                                    onChange={(event) => filterClients(event.target.value)}
                                />
                                <button
                                    type="button"
                                    className="close"
                                    style={{ padding: '0.3rem 0.3rem', marginLeft: '0.7rem' }}
                                    onClick={() => setFilteredClients(allClients)}
                                >
                                    <span aria-hidden="true">×</span>
                                </button>
                            </ListGroup.Item>
                            {filteredClients.map((client) => (
                                <ListGroup.Item
                                    key={client.ClientId}
                                    as="button"
                                    action
                                    variant={isClientSelected(client.ClientId) ? "secondary" : "light"}
                                    active={isClientSelected(client.ClientId)}
                                    onClick={(e) => toggleClient(e, client.ClientId)}
                                    style={{ display: 'flex' }}
                                >
                                    <label style={{ flex: 1 }}>{client.ClientCompanyName}</label>
                                    <div>
                                        <Badge
                                            variant={client.LastEvaluationCategory === "Promotor" ? "success" : (
                                                client.LastEvaluationCategory === "Neutro" ? "warning" : (
                                                    client.LastEvaluationCategory === "Detrator" ? "danger" : "light"
                                                ))}
                                            style={{ marginRight: 20 }}
                                        >{client.LastEvaluationCategory}</Badge>
                                        <label style={{ fontSize: 13 }}>Última avaliação: { client.LastEvaluationReferenceDate ? formatDate(client.LastEvaluationReferenceDate) : 'Não houve nenhuma' }</label>
                                    </div>
                                </ListGroup.Item>
                            ))}
                            {filteredClients.length === 0 && (
                                <ListGroup.Item>Não há nenhum cliente para selecionar</ListGroup.Item>
                            )}
                        </ListGroup>
                    </Form.Group>
                    

                    <h4 style={{ marginBottom: 30 }}>{`Avaliações dos clientes selecionados (${calculateSelectedClientPercentage()}% dos clientes selecionados)`}</h4>
                    {clientEvaluations.map((clientEvaluation) => (
                        <Card style={{ marginBottom: 20 }} key={clientEvaluation.clientId}>
                            <Card.Header>{`ID: ${clientEvaluation.clientId} - ${allClients.find((client) => client.ClientId === clientEvaluation.clientId).ClientCompanyName}`}</Card.Header>
                            <Card.Body>
                                <Form.Group controlId="formGrade">
                                    <Form.Label>Em uma escala de 0 a 10, qual a probabilidade de você recomendar nosso produto/serviço a um amigo/conhecido?</Form.Label>
                                    <Form.Control
                                        required
                                        type="number"
                                        placeholder="Digite a nota que você daria"
                                        onChange={(event) => editEvaluation(clientEvaluation.clientId, 'grade', event.target.value)}
                                        value={clientEvaluation.grade}
                                    />
                                </Form.Group>

                                <Form.Group controlId="formGrade">
                                    <Form.Label>Qual é o motivo dessa nota?</Form.Label>
                                    <Form.Control
                                        required
                                        type="text"
                                        placeholder="Digite a razão da nota"
                                        onChange={(event) => editEvaluation(clientEvaluation.clientId, 'reason', event.target.value)}
                                        value={clientEvaluation.reason}
                                    />
                                </Form.Group>
                            </Card.Body>
                        </Card>
                    ))}

                    {clientEvaluations.length === 0 && (
                        <Card style={{ marginBottom: 20 }}>
                            <Card.Header>Nenhum cliente foi selecionado</Card.Header>
                        </Card>
                    )}

                    {showAlert && (
                        <Alert variant={responseBody.message ? "success" : "danger"} onClose={() => setShowAlert(false)} dismissible>
                            <Alert.Heading>{responseBody.message ? 'Avaliação inserida com sucesso!' : 'Ocorreu algum erro :('}</Alert.Heading>
                            <p>
                            {responseBody.message ? responseBody.message : responseBody.error}
                            </p>
                        </Alert>
                    )}

                    <Button
                        variant="primary"
                        type="submit"
                        disabled={loading}
                        size="lg"
                        block
                    >
                        {loading ? 'Carregando...' : 'Salvar'}
                    </Button>
                </Form>
            </Jumbotron>
        </>
    );
}