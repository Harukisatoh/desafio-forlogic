import React, { useEffect, useState } from 'react';
import { Jumbotron, Table, Alert } from 'react-bootstrap';
import { useLocation } from "react-router-dom";

import Navbar from '../../Components/NavMenu'

export default function EvaluationResult() {
    const { state: { evaluation } } = useLocation();
    const [evaluationItems, setEvaluationItems] = useState([]);
    const [promotersCount, setPromotersCount] = useState(0);
    const [neutralCount, setNeutralCount] = useState(0);
    const [detractorsCount, setDetractorsCount] = useState(0);
    const [nps, setNps] = useState(0);
    const [evaluationResult, setEvaluationResult] = useState({});

    useEffect(() => {
        fetch(`http://localhost:50234/api/ClientEvaluation/${evaluation.EvaluationId}`)
            .then(response => response.json())
            .then(json => setEvaluationItems(json));
    }, []);

    useEffect(() => {
        let promotersCount = 0;
        let neutralCount = 0;
        let detractorsCount = 0;

        evaluationItems.forEach((item) => {
            if(item.Grade >= 9) {
                promotersCount++;
            } else if(item.Grade >= 7) {
                neutralCount++;
            } else {
                detractorsCount++;
            }
        });

        setPromotersCount(promotersCount);
        setNeutralCount(neutralCount);
        setDetractorsCount(detractorsCount);
    }, [evaluationItems]);

    useEffect(() => {
        const nps = ((promotersCount - detractorsCount) / evaluationItems.length) * 100;
        setNps(nps);
        
        if(nps >= 80) {
            setEvaluationResult({
                variant: 'success',
                message: 'Meta atingida!'
            });
        } else if (nps >= 60) {
            setEvaluationResult({
                variant: 'warning',
                message: 'Meta dentro da tolerância'
            });
        } else {
            setEvaluationResult({
                variant: 'danger',
                message: 'Meta não atingida :('
            });
        }
    }, [promotersCount, neutralCount, detractorsCount]);

    function formatDate(date) {
        const dt = new Date(date);
        const formattedDate = `${dt.getMonth() + 1}/${dt.getFullYear()}`;

        return formattedDate;
    }

    return(
        <>
            <Navbar />
            <Jumbotron style={{ height: '100vh' }}>
                <h1>Resultado da avaliação da data de {formatDate(evaluation.EvaluationReferenceDate)}</h1>

                <Table striped bordered hover>
                    <thead>
                        <tr>
                        <th>Participantes</th>
                        <th>Quantidade</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                        <td>Promotores</td>
                        <td>{promotersCount}</td>
                        </tr>
                        <tr>
                        <td>Neutros</td>
                        <td>{neutralCount}</td>
                        </tr>
                        <tr>
                        <td>Detratores</td>
                        <td>{detractorsCount}</td>
                        </tr>
                        <tr>
                        <td>Total</td>
                        <td>{evaluationItems.length}</td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                        <th>NPS</th>
                        <th>{nps.toPrecision(4)}</th>
                        </tr>
                    </tfoot>
                </Table>

                <Alert variant={evaluationResult.variant}>
                    {evaluationResult.message}
                </Alert>
            </Jumbotron>
        </>
    );
}