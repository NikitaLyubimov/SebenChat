import React, {Component} from 'react';

import login from './utils/login';
import "./LogInPage.scss";


export default class LogInPage extends Component{
    constructor(props){
        super(props);

        this.state = {
            userName: "",
            password: "",
            error: "",
            successLogin: false
        }

        this.handleChange = this.handleChange.bind(this);
        this.handleClick = this.handleClick.bind(this);
    }

    async handleClick(event){
        event.preventDefault();

        const {userName, password} = this.state;

        if(userName == "" || password == ""){
            this.setState({error: "All fields should be matched"});
            return;
        }

        var result = await login(userName, password);
        if(!result.success)
            alert(result.error.description);
        else{
            this.setState({seuccessLogin: true});
            alert(result.success);
        }
            

    }

    handleChange(event){
        const value = event.target.value;

        this.setState({
            ...this.state,
            [event.target.name]: value
        });
    }
    render(){
        return(
            <section>
               <div className="login-page">
                    <div className="form">
                        <p>Enter your account</p>
                        <form  className="login-form">
                            <input onChange={this.handleChange} name="userName" defaultValue={this.state.userName} type="text" placeholder="Username"/>
                            <input onChange={this.handleChange} name="password" defaultValue={this.state.password} type="password" placeholder="Password"/>
                            <button  onClick={this.handleClick} className="btn btn-success">Log In</button>
                        </form>
                        
                    </div>
                </div>
                
            </section>
        );
    }
}