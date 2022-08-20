import React, { useState } from 'react'
import Constants from '../Utilities/Constants'


export default function PostCreateForm() {
    return (
        <div>
            <form className="w-100 px-5">
                <h1 className="mt-5">Create New Post</h1>

                <div className='mt-5'>
                    <label className='h3 form-label'>Post Title</label>
                    <input value={formData.title} name="title" type="text" className='form-control'
                        onChange={handleChange} />
                </div>

                <div className='mt-4'>
                    <label className='h3 form-label'>Post Content</label>
                    <input value={formData.content} name="content" type="text" className='form-control'
                        onChange={handleChange} />
                </div>

                <button onClick={handleSubmit} className="btn btn-dark btn-lg w-100 mt-5">Submit</button>
                <button onClick={() => props.onPostCreate(null)} className="btn btn-secondary btn-lg w-100 mt-3">Cancel</button>
            </form>
        </div>
    )
}



