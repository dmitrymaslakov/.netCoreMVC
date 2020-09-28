import React from 'react';

const Post = (props) => {
  const { author, date, newsHeaderImage, body, headline, lead, reputation, source} = props;
  const { widthResult, progressColor, ariaValuenow } = getWidthProgressBar(reputation);
  return(
    <div className="post">
      <div className="p-2 mb-1 bg-white rounded" style={{display: 'flex'}}>
        <img style={{width: '250px', height: '125px'}} src={`data:image/jpeg;base64,${newsHeaderImage}`}/>
        <div style={{flex: 1, padding: '15px 30px'}}>
          <div className='progress ml-auto' style={{width: '200px', height: '20px'}}>
                <div className={`progress-bar ${progressColor}`} role="progressbar" style={{width: `${widthResult}`}} aria-valuenow={`${ariaValuenow}`} aria-valuemin="0" aria-valuemax="100"><strong>{reputation}</strong></div>
          </div>
          <h2>
            {headline}
          </h2>
          <p>{lead}</p>
          <details>
            <summary>Читать далее</summary>
            <p>{body}</p>
          </details>
          <p><strong>Автор статьи: <em>{author} </em><small className="text-muted">{date}</small></strong></p>
          <p><u><a href={`${source}`}>Источник</a></u></p>

        </div>
      </div>
    </div>
  );
  /*return (
    <div className="post">
      <div className="post__image" 
        style={{ backgroundImage: `url(data:image/jpeg;base64,${newsHeaderImage})` }}
      />
            <img style={{width: '250px', height: '125px', backgroundImage: `url(data:image/jpeg;base64,${newsHeaderImage})`}}/>

      <div className="post__info">
        <h2 className="post__title">{category}</h2>
      </div>
    </div>


  );*/
};
const getWidthProgressBar = (reputation) => {
  let widthResult = "";
  let progressColor = "";
  let ariaValuenow = 0;

  switch (reputation) {
    case -5:
      progressColor = "bg-danger";
      widthResult = "5%";
      ariaValuenow = 5;
      break;
    case -4:
      progressColor = "bg-danger";
      widthResult = "10%";
      ariaValuenow = 10;
      break;
    case -3:
      progressColor = "bg-warning";
      widthResult = "20%";
      ariaValuenow = 20;
      break;
    case -2:
      progressColor = "bg-warning";
      widthResult = "30%";
      ariaValuenow = 30;
      break;
    case -1:
      progressColor = "bg-warning";
      widthResult = "40%";
      ariaValuenow = 40;
      break;
    case 0:
      progressColor = "bg-info";
      widthResult = "50%";
      ariaValuenow = 50;
      break;
    case 1:
      progressColor = "bg-info";
      widthResult = "60%";
      ariaValuenow = 60;
      break;
    case 2:
      progressColor = "bg-primary";
      widthResult = "70%";
      ariaValuenow = 70;
      break;
    case 3:
      progressColor = "bg-primary";
      widthResult = "80%";
      ariaValuenow = 80;
      break;
    case 4:
      progressColor = "bg-success";
      widthResult = "90%";
      ariaValuenow = 90;
      break;
    case 5:
      progressColor = "bg-success";
      widthResult = "100%";
      ariaValuenow = 100;
      break;  
    }
return {
  widthResult,
  progressColor,
  ariaValuenow 
}
}
export default Post;