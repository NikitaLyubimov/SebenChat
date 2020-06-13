import React,{Component} from 'react';
import {BrowserRouter as Router, Route} from 'react-router-dom';

import MainPage from './components/MainPage/MainPage.jsx';
import SignUpPage from './components/SignUpPage/SignUpPage.jsx';
import ConfirmEmailPage from './components/ConfirmEmailPage/ConfirmEmailPage.jsx';

export default class App extends Component{
    render(){
        return(
            <Router>
                <Route exact path="/">
                    <MainPage/>
                </Route>
                <Route path="/sign-up">
                    <SignUpPage/>
                </Route>
                <Route path="/confirmation-message">
                    <ConfirmEmailPage/>
                </Route>
            </Router>
        );
    }
}