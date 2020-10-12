import { userConstants } from '../constants/user.constants';
import { urlConstants } from '../constants/url.constants';

export const userRegistrate = request => {

  let payload;
  fetch(urlConstants.USER_REGISTRATE, {  
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
    },
    body: JSON.stringify(request)
  })
    .then(resp => resp.json())
    .then(data => {
      if (data.message) {

      } else {
        payload = data.userName;
        localStorage.setItem('token', data.jwtToken);
        console.log(data);
      }
    })
  return {
    type: userConstants.USER_REGISTRATION,
    payload: payload
  }
}
