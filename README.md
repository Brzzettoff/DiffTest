Required features
• Provide 2 http endpoints (<host>/v1/diff/<ID>/left and <host>/v1/diff/<ID>/right) that accept
JSON containing base64 encoded binary data on both endpoints.
• The provided data needs to be diff-ed and the results shall be available on a third endpoint
(<host>/v1/diff/<ID>). The results shall provide the following info in JSON format:
o If equal return that
o If not of equal size just return that
o If of same size provide insight in where the diff are, actual diffs are not needed.
? So mainly offsets + length in the data
Note, that we are not looking for ideal diffing algorithm.
Make assumptions in the implementation explicit, choices are good but need to be communicated.
Use a Version Control System (preferably Git or Mercurial). Put the source code on a public repository
or send it zipped.
Requirements
• Preferably C#. If you are really not comfortable with C#, you can use some other objectoriented
and/or functional language, but provide more detailed info for running the solution
• Functionality shall be under integration tests (not full code coverage is required)
• Internal logic shall be under unit tests (not full code coverage is required)
• Documentation in code
• Short readme on usage
See the sample on next site.
Sample input/output
Request Response
1 GET /v1/diff/1 404 Not Found
2 PUT /v1/diff/1/left
{
"data": "AAAAAA=="
}
201 Created
3 GET /v1/diff/1 404 Not Found
4 PUT /v1/diff/1/right
{
"data": "AAAAAA=="
}
201 Created
5 GET /v1/diff/1 200 OK
{
"diffResultType": "Equals"
}
6 PUT /v1/diff/1/right
{
"data": "AQABAQ=="
}
201 Created
7 GET /v1/diff/1 200 OK
{
"diffResultType": "ContentDoNotMatch",
"diffs": [
 {
 "offset": 0,
 "length": 1
 },
 {
 "offset": 2,
 "length": 2
 }
]
}
8 PUT /v1/diff/1/left
{
 "data": "AAA="
}
201 Created
9 GET /v1/diff/1 200 OK
{
"diffResultType": "SizeDoNotMatch"
}
10 PUT /v1/diff/1/left
{
 "data": null
}
400