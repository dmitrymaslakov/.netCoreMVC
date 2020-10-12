import { userConstants } from '../constants/user.constants';
import jwt from 'jwt-decode';

const initialState = {
  currentUser: ''
}

/*if (localStorage.getItem('token')) {
  const tokenDecoded = jwt(localStorage.getItem('token'));
  console.log(tokenDecoded.exp * 1000);
  console.log(Date.now());
  if (tokenDecoded.exp * 1000 < Date.now()) {

    //localStorage.clear(); // this runs only when I refresh the page or reload on route change it dosent work
  } else {
    //initialState.currentUser = tokenDecoded;
    
  }
}*/

export default function reducer(state = initialState, action) {
  console.log(action);
  switch (action.type) {
    case userConstants.USER_LOGIN:
      return { ...state, currentUser: action.payload }
    case userConstants.USER_REGISTRATION:
      return { ...state, currentUser: action.payload }
    default:
      return state;
  }
}