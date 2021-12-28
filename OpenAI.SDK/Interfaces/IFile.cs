using OpenAI.SDK.Extensions;
using OpenAI.SDK.Models;
using OpenAI.SDK.Models.ResponseModels.FileResponseModels;
using OpenAI.SDK.Models.SharedModels;

namespace OpenAI.SDK.Interfaces;

public interface IFile
{
    /// <summary>
    ///     Returns a list of files that belong to the user's organization.
    /// </summary>
    /// <returns></returns>
    Task<FileListResponse> FileList();

    /// <summary>
    ///     Upload a file that contains document(s) to be used across various endpoints/features. Currently, the size of all
    ///     the files uploaded by one organization can be up to 1 GB. Please contact us if you need to increase the storage
    ///     limit.
    /// </summary>
    /// <param name="file">
    ///     Name of the <a href="https://jsonlines.readthedocs.io/en/latest/">JSON Lines</a> file to be uploaded.
    ///     If the purpose is set to "search" or "answers", each line is a JSON record with a "text" field and an optional
    ///     "metadata" field.Only "text" field will be used for search.Specially, when the purpose is "answers", "\n" is used
    ///     as a delimiter to chunk contents in the "text" field into multiple documents for finer-grained matching.
    ///     If the purpose is set to "classifications", each line is a JSON record representing a single training example with
    ///     "text" and "label" fields along with an optional "metadata" field.
    ///     If the purpose is set to "fine-tune", each line is a JSON record with "prompt" and "completion" fields representing
    ///     your <a href="https://beta.openai.com/docs/guides/fine-tuning/prepare-training-data">training examples</a>.
    /// </param>
    /// <param name="fileName">Name of file</param>
    /// <param name="purpose">
    ///     The intended purpose of the uploaded documents.
    ///     Use "search" for Search, "answers" for Answers, "classifications" for Classifications and "fine-tune" for
    ///     Fine-tuning.This allows us to validate the format of the uploaded file.
    /// </param>
    /// <returns></returns>
    Task<FileUploadResponse> FileUpload(string purpose, byte[] file, string fileName);

    Task<FileUploadResponse> FileUpload(string purpose, Stream file, string fileName)
    {
        return FileUpload(purpose, file.ToByteArray(), fileName);
    }

    Task<FileUploadResponse> FileUpload(UploadFilePurposes.UploadFilePurpose purpose, Stream file, string fileName)
    {
        return FileUpload(purpose.EnumToString(), file.ToByteArray(), fileName);
    }

    Task<FileUploadResponse> FileUpload(UploadFilePurposes.UploadFilePurpose purpose, byte[] file, string fileName)
    {
        return FileUpload(purpose.EnumToString(), file, fileName);
    }

    /// <summary>
    ///     Delete a file.
    /// </summary>
    /// <param name="fileId">The ID of the file to use for this request</param>
    /// <returns></returns>
    Task<FileDeleteResponse> FileDelete(string fileId);

    /// <summary>
    ///     Returns information about a specific file.
    /// </summary>
    /// <param name="fileId">The ID of the file to use for this request</param>
    /// <returns></returns>
    Task<FileResponse> FileRetrieve(string fileId);

    /// <summary>
    ///     Returns the contents of the specified file
    /// </summary>
    /// <param name="fileId">The ID of the file to use for this request</param>
    /// <returns></returns>
    Task FileRetrieveContent(string fileId);
}