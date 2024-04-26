export const replaceIdInURL = (url: string, id: unknown) => {
	return url.replace(":id", id as string);
};
