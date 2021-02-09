import React, { useEffect, useState } from 'react';
import Navbar from '../../Components/NavMenu';
import { Jumbotron, ListGroup, Button } from 'react-bootstrap';
import { useHistory } from 'react-router-dom';

export default function Evaluations() {
    const [evaluations, setEvaluations] = useState([]);
    const history = useHistory();

    useEffect(() => {
        fetch('http://localhost:50234/api/Evaluation')
            .then(response => response.json())
            .then(json => setEvaluations(json));
    }, []);

    function formatDate(date) {
        const dt = new Date(date);
        const formattedDate = `${dt.getMonth() + 1}/${dt.getFullYear()}`;

        return formattedDate;
    }

    function handleRedirectToEvaluation(evaluation) {
        history.push({
            pathname: '/avaliacao',
            state: { evaluation }
        });
    }

    function handleNavigationToCreateEvaluation() {
        history.push('/avaliacao/criar');
    }

    return (
        <>
            <Navbar />
            <Jumbotron style={{ height: '100%' }}>
                <div style={{ display: 'flex' }}>
                    <h1 style={{ flex: 1 }}>Avaliações</h1>

                    <div style={{ paddingTop: '1rem', paddingBottom: '1rem' }}>
                        <Button variant="success" onClick={handleNavigationToCreateEvaluation}>Nova avaliação</Button>
                    </div>
                </div>
                <ListGroup>
                    {evaluations.map((evaluation) => (
                        <ListGroup.Item
                            key={evaluation.EvaluationId}
                            as="button"
                            action
                            variant="light"
                            onClick={() => handleRedirectToEvaluation(evaluation)}
                            style={{ display: 'flex' }}
                        >
                            <label style={{ flex: 1 }}>{`ID: ${evaluation.EvaluationId} - Avaliação referente a data de ${formatDate(evaluation.EvaluationReferenceDate)}`}</label>
                        </ListGroup.Item>
                    ))}
                </ListGroup>
            </Jumbotron>
        </>
    );
}