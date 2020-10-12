import React, { Component } from "react";
import { Route, Switch, Redirect, } from "react-router-dom";
import { getProfileFetch } from './actions/getProfileFetch';
import Post from "./components/Post";
import {Login} from "./components/Login";
import { urlConstants } from './constants/url.constants';
import jwt from 'jwt-decode';
import axios from "axios";
import { connect } from "react-redux";
import Cookies from 'js-cookie';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      data: [],
    }
  }
  componentDidMount() {
    //localStorage.clear();
    axios.get(urlConstants.GET_NEWS).then(({ data }) => {
      this.setState({ data });
    });
  }


  render() {
    return (
      <div>
        {
          !this.state.data.length ? (<span>Loading...</span>) :
          (this.state.data.map((prop, key) => {
            return (
              <Post
              key={key}
              author = {prop.author}
              date = {prop.date}
              newsHeaderImage={prop.newsHeaderImage}
              body={prop.newsStructure.body}
              headline={prop.newsStructure.headline}
              lead={prop.newsStructure.lead}
              reputation={prop.reputation}
              source={prop.source}
              />
            )
          }))
        }
      </div>
    );
    /*let token = localStorage.getItem('token');
    let b;
    if (token !== null && token !== '') {
      b = isTokenExpired(token);
    }
    else {
      b = true;
    }
    let b = false;
    let res = <div></div>
    if (b) {
      res =
        <div>
          <Switch>
            <Route path='/login' component={Login} />
            <Redirect from='/' to='/login' />
          </Switch>
        </div>
    }
    else {
      res =
      <div>
          {
            !this.state.data.length ? (<span>Loading...</span>) :
              (this.state.data.map((prop, key) => {
                return (
                  <Post
                    key={key}
                    author={prop.author}
                    date={prop.date}
                    newsHeaderImage={prop.newsHeaderImage}
                    body={prop.newsStructure.body}
                    headline={prop.newsStructure.headline}
                    lead={prop.newsStructure.lead}
                    reputation={prop.reputation}
                    source={prop.source}
                  />
                )
              }))
          }
        </div>
    }
    return res;*/
  }
}
/*const state = props => {
  return { loading: true, ...props};
};
const actions = dispatch => ({
  setPosts: (data) => dispatch({
    type: 'SET_POSTS',
    payload: data
  }) 
});*/

/*const mapStateToProps = state => (
  {
  newsList: state.items
})*/

/*const mapStateToProps = state => {
  return {
    newsList: state.posts.items
  }
}*/
const isTokenExpired = token => {
  const tokenDecoded = jwt(token);
  if (tokenDecoded.exp * 1000 < Date.now()) {
    return true;
  } else {
    return false;
  }
}

const mapStateToProps = state => {
  return {
    currentUser: state.currentUser
  }
}


const mapDispatchToProps = dispatch => ({
  getProfileFetch: () => dispatch(getProfileFetch())
})

const connectedApp = connect(mapStateToProps, mapDispatchToProps)(App);
export { connectedApp as App };
//export default connect(mapStateToProps, mapActionToProps)(App);
