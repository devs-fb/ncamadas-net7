// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pendente>")]
[assembly: SuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pendente>")]
[assembly: SuppressMessage("Usage", "CA2254:O modelo deve ser uma expressão estática", Justification = "<Pendente>")]
[assembly: SuppressMessage("Design", "CA1050:Declarar tipos em namespaces", Justification = "<Pendente>", Scope = "type", Target = "~T:WebhookPayload")]
