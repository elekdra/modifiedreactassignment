/* eslint-disable react-hooks/exhaustive-deps */
import AdminHeader from '../AdminDashboard/AdminHeader/AdminHeader';
import Back from './Back/Back';
import Form from './Form/Form';
import DocumentTable from './DocumentTable/DocumentTable';
import { useState, useEffect } from 'react';
import { StartUpDefaultsValue } from '../../ApiServices/ApiServices';

// training documents main components here
const Index = (props) => {
  const [fullfileData, setFullFileData] = useState();
  let fullData;
  const fetchData = () => {
    StartUpDefaultsValue().then((response) => {
      fullData = response.data;
      fullData.forEach((item) => {
        let result = item.fileContent.indexOf("files");
        let tempName=item.fileContent.slice(result+5,item.fileContent.length);
         item.fileContent = 'http://localhost:5000/files/' + tempName;
      });
      setFullFileData(fullData);
    });
  };
  useEffect(() => {
    fetchData();
  }, []);

  return (
    <div className='training'>
      <Back page='/trainingdocuments'  username={props.username}/>
      <AdminHeader
        style={{ marginTop: '1rem', padding: '1rem', borderTopColor: 'white' }}
        className='doc-header'
        title='Training Documents'
      />
      <Form item={props} fullfileData={fullfileData} setFullFileData={setFullFileData} />
      <DocumentTable
        fullData={fullfileData}
        onDeleted={() => {
          fetchData();
        }}
      />
    </div>
  );
};

export default Index;
