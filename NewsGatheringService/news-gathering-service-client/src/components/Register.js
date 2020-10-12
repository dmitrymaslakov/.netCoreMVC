import React from 'react';
import { Component } from "react";
import { connect } from 'react-redux';
import { userRegistrate } from '../actions/userRegisterate';

class Register extends Component {
    state = {
        userlogin: "",
        password: "",
    }

    handleChange = e => {
        const { name, value } = e.target;
        this.setState({ [name]: value });
    }

    handleSubmit = event => {
        const user = {
            login: this.state.userlogin,
            password: this.state.password
        };

        event.preventDefault()
        this.props.userRegistrate(user)
        console.log('currentUser', this.props.currentUser);
    }

    render() {
        return (
            <form onSubmit={this.handleSubmit}>
                <h4>Регистрация на сайте</h4>
                <hr></hr>
                <div className="form-group">
                    <label htmlFor="userlogin">User Login</label>
                    <input type="text" className="form-control"
                        name='userlogin'
                        placeholder='User Login'
                        value={this.state.username}
                        onChange={this.handleChange}
                    /><br />
                </div>

                <div className="form-group">
                    <label>Password</label>
                    <input className="form-control"
                        type='password'
                        name='password'
                        placeholder='Password'
                        value={this.state.password}
                        onChange={this.handleChange}
                    /><br />

                </div>

                <input type='submit' />
            </form>
        )
    }
}
const mapStateToProps = state => {
    return {
        currentUser: state.currentUser
    }
}

const mapDispatchToProps = dispatch => ({
    userRegistrate: userInfo => dispatch(userRegistrate(userInfo))
})

const connectedRegister = connect(mapStateToProps, mapDispatchToProps)(Register);
export { connectedRegister as Register };