import React, { useState } from 'react';
import Navbar from '../../Components/NavMenu';
import { Jumbotron, Form, Button, Alert } from 'react-bootstrap';

export default function Clients() {
    const [showAlert, setShowAlert] = useState(false);
    const [responseBody, setResponseBody] = useState('');
    const [loading, setLoading] = useState(false);
    const [clientCompanyName, setClientCompanyName] = useState('');
    const [clientContactName, setClientContactName] = useState('');
    const [clientCNPJ, setClientCNPJ] = useState('');

    function formatField() {
        var x = clientCNPJ.replace(/\D/g, '').match(/(\d{0,2})(\d{0,3})(\d{0,3})(\d{0,4})(\d{0,2})/);
        const formattedValue = !x[2] ? x[1] : x[1] + '.' + x[2] + '.' + x[3] + '/' + x[4] + (x[5] ? '-' + x[5] : '');
    
        return formattedValue;
    }

    function unformatField() {
        const unformattedValue = clientCNPJ.replace(/(\.|\/|\-)/g,"");
        
        return unformattedValue;
    }

    function formSubmit(e) {
        e.preventDefault();
        const unformattedClientCNPJ = unformatField();

        setLoading(true);
        fetch('http://localhost:50234/api/Client', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                clientCompanyName,
                clientContactName,
                clientCNPJ: unformattedClientCNPJ
            })
        }).then(response => response.json())
        .then(json => {
            setResponseBody(json);
            setShowAlert(true);
            setLoading(false);
        });
    }

    return (
        <>
            <Navbar />
            <Jumbotron style={{ height: '100vh' }}>
                <h1>Cadastro de cliente</h1>
                <Form autoComplete="off" onSubmit={formSubmit}>
                    <Form.Group controlId="formClientCompanyName">
                        <Form.Label>Nome do cliente</Form.Label>
                        <Form.Control
                            required
                            type="text"
                            placeholder="Digite a razão social/nome fantasia"
                            onChange={(event) => setClientCompanyName(event.target.value)}
                        />
                    </Form.Group>

                    <Form.Group controlId="formClientContactName">
                        <Form.Label>Nome da pessoa de contato</Form.Label>
                        <Form.Control
                            required
                            type="text"
                            placeholder="Nome do responsável"
                            onChange={(event) => setClientContactName(event.target.value)}
                        />
                    </Form.Group>

                    <Form.Group controlId="formClientCNPJ">
                        <Form.Label>CNPJ do cliente</Form.Label>
                        <Form.Control
                            type="text"
                            placeholder="Digite o CNPJ do cliente"
                            onChange={(event) => setClientCNPJ(event.target.value)}
                            value={clientCNPJ}
                            maxLength={14}
                            onFocus={() => setClientCNPJ(unformatField())}
                            onBlur={() => setClientCNPJ(formatField())}
                        />
                    </Form.Group>

                    {showAlert && (
                        <Alert variant={responseBody.message ? "success" : "danger"} onClose={() => setShowAlert(false)} dismissible>
                            <Alert.Heading>{responseBody.message ? 'Cliente inserido com sucesso!' : 'Ocorreu algum erro :('}</Alert.Heading>
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