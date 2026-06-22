<%

'response.write request("userno")
'response.end
response.redirect "http://e-rma.cipherlab.com.tw:8091/warr_ext_qry.aspx?userno=" & request("userno")
%>