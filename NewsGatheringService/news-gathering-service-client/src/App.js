import React, { Component } from "react";
import Post from "./components/Post";
import axios from "axios";
import { connect } from "react-redux";
import posts from "./reducers/posts.js";
import * as actions from "./actions/news";

/*import { postsActions } from "./actions/posts";
import { appActions } from "./actions/app";

import { Header } from "./components";

import posts from './posts.json';*/



class App extends Component {
  constructor(props){
    super(props);
    this.state = {  
      data: []
    }
  }
  /*componentWillMount(){
    axios.get('https://localhost:5001/api/news').then(({data})=>{
      this.setState({data});
    })
  }*/
    componentWillMount(){
      this.props.fetchAllNews();
      console.log(this.state);
    }
  

  render() {
    console.log(this.props);
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

const mapStateToProps = state => {
  return{
    newsList: state.posts.items
  }
}

const mapActionToProps = {
  fetchAllNews: actions.fetchAll
}


export default connect(mapStateToProps, mapActionToProps)(App);
