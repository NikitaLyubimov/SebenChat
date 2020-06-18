import React, {Component} from 'react';
import {Redirect} from 'react-router-dom';

import './ErrorForm.jsx';
import './SignUpPage.scss';
import registerPost from './utils/registration';
import ErrorForm from './ErrorForm.jsx';

export default class SignUpPage extends Component{

    constructor(props){
        super(props);
        this.state = {
            firstName: "",
            secondName: "",
            userName: "",
            email: "",
            password: "",
            formValid: false,
            error: "",
            redirect: false
        }
        this.handleChange = this.handleChange.bind(this);
        this.handleClick = this.handleClick.bind(this);
        this.renderRedirect = this.renderRedirect.bind(this);
    }

    async handleClick(event){
        event.preventDefault();

        const {firstName, secondName, userName, email,password} = this.state;

        if(firstName === "" || secondName === "" || userName === "" || email === "" || password === ""){
            this.setState({error: "All fields should be matched"});
            return;
        }
        var result = await registerPost(firstName, secondName, userName, email, password);
        if(!result.success)
            alert(result.errors[0]);
        else
            this.setState({redirect: true})

        console.log(this.state.redirect);
    }

    handleChange(evt){
        
        const value = evt.target.value;
        this.setState({
            ...this.state,
            [evt.target.name]: value
        });

        if(evt.target.name == "password")
        {
            let reg = new RegExp("[^a-zA-Z0-9]");
            const isOk = reg.test(value);

            if(!isOk || value.length < 10)
                this.setState({formValid: false, error: "Password must contains non alphanumeric characters and be more than 10 characters"});
            else
                this.setState({formValid: true, error: ""});
        }

    }

    renderRedirect = () => {
        if(this.state.redirect){
            console.log(this.state.redirect);

            return <Redirect to='/confirmation-message'/>
        }
    }



    
    render(){
        const Redirection = this.renderRedirect();
        return(
            <section>
               <div className="login-page">
                    <div className="form">
                        <p>Registration</p>
                        <form  className="login-form">
                            <input onChange={this.handleChange} name="firstName" defaultValue={this.state.firstName} type="text" placeholder="First name"/>
                            <input onChange={this.handleChange} name="secondName" defaultValue={this.state.secondName} type="text" placeholder="Second name"/>
                            <input onChange={this.handleChange} name="userName" defaultValue={this.state.userName} type="text" placeholder="Username"/>
                            <input onChange={this.handleChange} name="email" defaultValue={this.state.email} type="text" placeholder="Email"/>
                            <input onChange={this.handleChange} name="password" defaultValue={this.state.password} type="password" placeholder="Password"/>
                            {Redirection}
                            <button  onClick={this.handleClick} className="btn btn-success">Sign Up</button>
                        </form>
                        
                        <ErrorForm error={this.state.error}/>
                    </div>
                </div>
                
            </section>
        );
    }
}