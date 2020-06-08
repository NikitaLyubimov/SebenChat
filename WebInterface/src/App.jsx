import React,{Component} from 'react';
import MainPage from './components/MainPage/MainPage.jsx';
import {BrowserRouter as Router, Route} from 'react-router-dom';

export default class App extends Component{
    render(){
        return(
            <Router>
                <Route exact path="/">
                    <MainPage/>
                </Route>
            </Router>
        );
    }
}