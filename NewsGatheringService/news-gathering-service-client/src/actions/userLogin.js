import { userConstants } from '../constants/user.constants';
import { urlConstants } from '../constants/url.constants';
import React from 'react';
import Cookies from 'js-cookie';

export const userLogin = request => {
  let payload;
  fetch(urlConstants.USER_AUTHENTICATE, {  
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
    },
    body: JSON.stringify(request)
  })
    .then(resp => {
        console.log(resp.json());
        resp.json();
    })
    .then(data => {
      if (data.message) {
        console.log(data);
        console.log(data.message);
      } else {
        payload = data.userName;
        localStorage.setItem('token', data.jwtToken);
        console.log(data);
      }
    })
  return {
    type: userConstants.USER_LOGIN,
    payload: payload
  }
}
