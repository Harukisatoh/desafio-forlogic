  
import React from 'react';
import { Route, BrowserRouter } from 'react-router-dom';

import Evaluations from './pages/Evaluations';
import Clients from './pages/Clients';
import CreateEvaluation from './pages/CreateEvaluation';
import EvaluationResult from './pages/EvaluationResult';

const Routes = () => {
    return (
        <BrowserRouter>
            <Route component={Evaluations} path='/' exact />
            <Route component={EvaluationResult} path='/avaliacao' exact />
            <Route component={CreateEvaluation} path='/avaliacao/criar' />
            <Route component={Clients} path='/clientes' />
        </BrowserRouter>
    );
}

export default Routes;